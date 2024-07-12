namespace MyLab.EmailManager.Infrastructure.MessageTemplates
{
    public interface IMessageTemplateService
    {
        Task<string> CreateTextContentAsync(string templateId, IDictionary<string, string> args);
    }
}
