using Microsoft.Extensions.Options;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.App.ConfirmationStuff
{
    public class ConfirmationMessageSender
    (
        IMailMessageSender mailMessageSender,
        IOptions<ConfirmationOptions> opts)
    {
        public async Task SendAsync(Confirmation confirmation, string address, TemplateContext tCtx, CancellationToken cancellationToken)
        {
            await mailMessageSender.SendMessageAsync
                (
                    address, 
                    opts.Value.Subject,
                    ConfirmationMessageConstants.TemplateId, 
                    tCtx, 
                    cancellationToken
                );

            confirmation.ToSentState();
        }
    }
}
