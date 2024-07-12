using Microsoft.Extensions.Options;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.App.ConfirmationStuff
{
    public class ConfirmationMessageSender
    (
        IMailMessageSender mailMessageSender,
        IOptions<ConfirmationOptions> opts)
    {
        public Task SendAsync(string address, IReadOnlyDictionary<string,string>? args)
        {
            return mailMessageSender.SendMessageAsync(address, opts.Value.Subject, ConfirmationMessageConstants.TemplateId, args);
        }
    }
}
