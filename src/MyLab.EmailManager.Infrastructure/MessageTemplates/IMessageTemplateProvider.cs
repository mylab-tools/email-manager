namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public interface IMessageTemplateProvider
{
    Task<string> ProvideAsync(string templateId);
}