using MyLab.EmailManager.Infrastructure.MessageTemplates;

namespace MyLab.EmailManager.Infrastructure.Messaging;

public interface IMailMessageSender
{
    Task SendMessageAsync(string to, string subject, string templateId, TemplateContext tCtx, CancellationToken cancellationToken);
}