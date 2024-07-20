using MediatR;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.MailServer;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;

namespace MyLab.EmailManager.App.Features.SendPendingMessages
{
    public class SendPendingMessagesHandler
        (
            ISendingRepository repo, 
            IMailServerIntegration mailServerIntegration,
            ILogger<SendPendingMessagesHandler>? log = null
        ) : IRequestHandler<SendPendingMessagesCommand>
    {
        readonly IDslLogger? _log = log?.Dsl();
        public async Task Handle(SendPendingMessagesCommand request, CancellationToken cancellationToken)
        {
            var sendings = await repo.GetAsync(s => s.SendingStatus.Value != SendingStatus.Sent, cancellationToken);

            foreach (var sending in sendings)
            {
                if(sending.Messages == null) continue;

                sending.SendingStatus = SendingStatus.Sending;
                await repo.SaveAsync(cancellationToken);

                var pendingMessages = sending.Messages
                    .Where(m => m.SendingStatus.Value == SendingStatus.Pending);

                foreach (var pendingMessage in pendingMessages)
                {
                    await SendMessageAsync(pendingMessage, cancellationToken);
                }

                if (sending.Messages.All(m => m.SendingStatus.Value == SendingStatus.Sent))
                    sending.SendingStatus = SendingStatus.Sent;
                else if (sending.Messages.Any(m => m.SendingStatus.Value == SendingStatus.Sent))
                    sending.SendingStatus = SendingStatus.Sending;
                else if (sending.Messages.All(m => m.SendingStatus.Value == SendingStatus.Pending))
                    sending.SendingStatus = SendingStatus.Pending;
                else
                {
                    sending.SendingStatus = SendingStatus.Undefined;

                    _log?.Warning("the sending messages has wrong status combination")
                        .AndFactIs("sending-id", sending.Id)
                        .Write();
                }
            }

            await repo.SaveAsync(cancellationToken);
        }

        private async Task SendMessageAsync(EmailMessage message, CancellationToken cancellationToken)
        {
            message.SendingStatus = SendingStatus.Sending;

            try
            {
                await mailServerIntegration.SendMessageAsync
                (
                    message.EmailAddress.Value,
                    message.Title.Text,
                    message.Content,
                    cancellationToken
                );

                message.SendingStatus = SendingStatus.Sent;
            }
            catch (Exception e)
            {
                message.SendingStatus = SendingStatus.Pending;
                _log?.Error("Message sending error", e).Write();
            }
        }
    }
}
