using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates
{
    public interface IMessageTemplateService
    {
        Task<TextContent> CreateTextContentAsync(string templateId, IReadOnlyDictionary<string, string>? args);
    }
}
