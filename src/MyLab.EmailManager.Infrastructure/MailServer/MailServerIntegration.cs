using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.Infrastructure.MailServer;

public class MailServerIntegration(SmtpOptions opts) : IMailServerIntegration
{
    public MailServerIntegration(IOptions<SmtpOptions> opts)
        :this(opts.Value)
    {

    }

    public Task SendMessageAsync(string toAddress, string subject, TextContent textContent)
    {
        var smtp = new SmtpClient
        {
            Host = opts.Host!,
            Port = opts.Port,
            Credentials = new NetworkCredential(opts.Login, opts.Password)
        };

        var from = new MailAddress
            (
                opts.SenderEmail!,
                opts.SenderName
            );

        var to = new MailAddress(toAddress);

        var message = new MailMessage(from, to)
        {
            Subject = subject,
            Body = textContent.Content,
            IsBodyHtml = textContent.IsHtml
        };

        return smtp.SendMailAsync(message);
    }
}