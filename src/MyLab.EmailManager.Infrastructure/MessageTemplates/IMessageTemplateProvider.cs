using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public interface IMessageTemplateProvider
{
    Task<TextContent> ProvideAsync(string templateId, CancellationToken cancellationToken);
}