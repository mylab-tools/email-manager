using Microsoft.Extensions.Options;
using MyLab.EmailManager.Infrastructure.MailServer;
using MyLab.EmailManager.Infrastructure.MessageTemplates;

namespace MyLab.EmailManager.Infrastructure.Messaging
{
    public class MailMessageSender 
    (
        IMessageTemplateService messageTemplateService,
        IMailServerIntegration mailServerIntegration
    ) : IMailMessageSender
    {
        public async Task SendMessageAsync(string to, string subject, string templateId, TemplateContext tCtx, CancellationToken cancellationToken)
        {
            var txtContent = await messageTemplateService.CreateTextContentAsync(templateId, tCtx, cancellationToken);

            if (txtContent == null)
                throw new InvalidOperationException("Message content is null");

            await mailServerIntegration.SendMessageAsync(to, subject, txtContent, cancellationToken);
        }
    }
}
