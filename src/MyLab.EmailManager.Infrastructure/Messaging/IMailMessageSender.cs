namespace MyLab.EmailManager.Infrastructure.Messaging;

public interface IMailMessageSender
{
    Task SendMessageAsync(string to, string subject, string templateId, IReadOnlyDictionary<string, string>? args);
}