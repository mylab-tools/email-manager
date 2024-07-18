using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.EmailManager.Infrastructure.MessageTemplates;

namespace MyLab.EmailManager.App.Features.CreateSending
{
    public class CreateSendingHandler
        (
            ISendingRepository sendingRepo,
            ReadDbContext dbContext,
            IMessageTemplateService messageTemplateService,
            ILogger<CreateSendingHandler> log
        ) : IRequestHandler<CreateSendingCommand, CreateSendingResponse>
    {
        public async Task<CreateSendingResponse> Handle(CreateSendingCommand request, CancellationToken cancellationToken)
        {
            var emailLabelPairs = await dbContext.Labels
                .Where(WhereDbLabel.In(request.Selection))
                .Select(l => new
                {
                    EmailId = l.Email.Id, 
                    Addr = l.Email.Address, 
                    LabelName = l.Name, 
                    LabelValue = l.Value
                })
                .GroupBy(p => p.EmailId)
                .ToDictionaryAsync(g => g.Key, g => g.ToArray(), cancellationToken);

            var emails = emailLabelPairs
                .Where(p => p.Value.Length != 0)
                .Where(p =>
                    request.Selection.All
                    (
                        kv => p.Value.Any
                        (
                            ep => ep.LabelName == kv.Key && ep.LabelValue == kv.Value
                        )
                    )
                )
                .Select(ep => ep.Value.First())
                .ToArray();

            if (emails.Length == 0)
                log.LogWarning("No email found for sending");

            var sendingId = Guid.NewGuid();
            var selection = request.Selection
                .Select(kv => new EmailLabel(kv.Key, kv.Value))
                .ToArray();

            Sending newSending;

            if (request.SimpleContent != null)
            {
                newSending = new Sending(sendingId, selection, request.SimpleContent)
                {
                    Messages = emails.Select
                    (
                        email => EmailMessage.New
                        (
                            email.EmailId,
                            email.Addr,
                            request.Title,
                            new TextContent(request.SimpleContent, false)
                        )
                    ).ToList()
                };
            }
            else if (request.TemplateId != null)
            {
                var messages = new List<EmailMessage>();

                var emailsLabels = await dbContext.Emails
                    .Where(email => emails.Any(e => e.EmailId == email.Id))
                    .Include(email => email.Labels)
                    .ToDictionaryAsync(email => email.Id, email => email.Labels, cancellationToken);

                foreach (var email in emails)
                {
                    emailsLabels.TryGetValue(email.EmailId, out var emailLabels);
                    var emailLabelsDict = emailLabels
                        ?.ToDictionary(l => l.Name, l => l.Value!)
                        .AsReadOnly();

                    var msgContent = await messageTemplateService.CreateTextContentAsync
                        (
                            request.TemplateId,
                            new TemplateContext(request.TemplateArgs, emailLabelsDict),
                            cancellationToken
                        );
                    messages.Add
                        (
                            EmailMessage.New
                            (
                                email.EmailId,
                                email.Addr,
                                request.Title,
                                msgContent
                            )
                        );
                }

                newSending = new Sending(sendingId, selection, request.TemplateId, request.TemplateArgs)
                    {
                        Messages = messages
                    };
            }
            else throw new InvalidOperationException("Message content is required");

            newSending.SendingStatus = SendingStatus.Pending;

            sendingRepo.Add(newSending);
            await sendingRepo.SaveAsync(cancellationToken);

            return new CreateSendingResponse(sendingId);
        }
    }
}