using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public interface IMessageTemplateProvider
{
    Task<TextContent> ProvideAsync(string templateId);
}