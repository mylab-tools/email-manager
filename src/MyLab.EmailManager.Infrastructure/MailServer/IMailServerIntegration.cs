using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.Infrastructure.MailServer
{
    public interface IMailServerIntegration
    {
        Task SendMessageAsync(string toAddress, string subject, TextContent textContent, CancellationToken cancellationToken);
    }
}
