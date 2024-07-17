using MediatR;
using MyLab.EmailManager.App.ConfirmationStuff;
using MyLab.EmailManager.App.Exceptions;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Infrastructure.MessageTemplates;

namespace MyLab.EmailManager.App.Features.RepeatConfirmation
{
    public class RepeatConfirmationHandler
        (
            IEmailRepository repo,
            ConfirmationMessageSender messageSender
        ) 
        : IRequestHandler<RepeatConfirmationCommand>
    {
        public async Task Handle(RepeatConfirmationCommand request, CancellationToken cancellationToken)
        {
            var email = await repo.GetAsync(request.EmailId, cancellationToken);
            if (email == null)
                throw new NotFoundException("Email not found");

            if (email.Confirmation != null)
            {
                email.Confirmation.Reset();
            }
            else
            {
                email.Confirmation = Confirmation.CreateNew(request.EmailId);
            }

            await repo.SaveAsync(cancellationToken);

            await messageSender.SendAsync
            (
                email.Address.Value, 
                new TemplateContext
                    (
                        email.Labels.ToDictionary
                        (
                            l => l.Name.Text,
                            l => l.Value
                        ).AsReadOnly(),
                        null
                    ),
                cancellationToken
            );
        }
    }
}
