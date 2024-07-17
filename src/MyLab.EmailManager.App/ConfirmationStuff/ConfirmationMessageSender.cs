using Microsoft.Extensions.Options;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.App.ConfirmationStuff
{
    public class ConfirmationMessageSender
    (
        IMailMessageSender mailMessageSender,
        IOptions<ConfirmationOptions> opts)
    {
        public Task SendAsync(string address, TemplateContext tCtx, CancellationToken cancellationToken)
        {
            return mailMessageSender.SendMessageAsync(address, opts.Value.Subject, ConfirmationMessageConstants.TemplateId, tCtx, cancellationToken);
        }
    }
}
