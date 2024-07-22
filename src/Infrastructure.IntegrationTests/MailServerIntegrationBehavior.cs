using System.Text;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.MailServer;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace Infrastructure.IntegrationTests
{
    public class MailServerIntegrationBehavior : IAsyncLifetime
    {
        const string ServerHost ="localhost";
        const string ServerLogin = "test-login";
        const string ServerPassword = "test-password";

        private const string MailFromName = "Test";
        private const string MailFromAddress = "test@host.com";

        readonly SmtpOptions _smtpOptions = new()
        {
            Host = ServerHost,
            Port = 10162,
            Login = ServerLogin,
            Password = ServerPassword,
            SenderEmail = MailFromAddress,
            SenderName = MailFromName
        };

        [Fact]
        public async Task ShouldSendSimpleMessage()
        {
            //Arrange
            var integration = new MailServerIntegration(_smtpOptions);

            //Act
            await integration.SendMessageAsync
            (
                "target@host.com", 
                "foo", 
                new TextContent("bar", false),
                CancellationToken.None
            );

            var gotMessage = await ReadLastMessageAsync();
            TextPart? textBody = null;

            if (gotMessage?.Body != null)
                textBody = gotMessage.Body as TextPart;

            //Assert
            Assert.NotNull(gotMessage);
            Assert.NotNull(gotMessage.Body);
            Assert.StartsWith("text/plain",gotMessage.Body.Headers[HeaderId.ContentType]);
            Assert.Equal("foo",gotMessage.Subject);
            Assert.Single(gotMessage.From);
            Assert.Equal(MailFromName, gotMessage.From.First().Name);
            Assert.Equal
            (
                $"\"{MailFromName}\" <{MailFromAddress}>", 
                gotMessage.From.First().ToString()
            );

            Assert.NotNull(textBody);
            Assert.Equal("bar", textBody.Text.Trim());
            Assert.False(textBody.IsHtml);
        }

        [Fact]
        public async Task ShouldSendHtmlMessage()
        {
            //Arrange
            var integration = new MailServerIntegration(_smtpOptions);

            //Act
            await integration.SendMessageAsync
            (
                "target@host.com",
                "foo",
                new TextContent("<html><body>Ololo</body></html>", true),
                CancellationToken.None
            );

            var gotMessage = await ReadLastMessageAsync();
            TextPart? textBody = null;

            if (gotMessage?.Body != null)
                textBody = gotMessage.Body as TextPart;

            //Assert
            Assert.NotNull(gotMessage);
            Assert.NotNull(gotMessage.Body);
            Assert.StartsWith("text/html", gotMessage.Body.Headers[HeaderId.ContentType]);
            Assert.Equal("foo", gotMessage.Subject);
            Assert.Single(gotMessage.From);
            Assert.Equal(MailFromName, gotMessage.From.First().Name);
            Assert.Equal
            (
                $"\"{MailFromName}\" <{MailFromAddress}>",
                gotMessage.From.First().ToString()
            );

            Assert.NotNull(textBody);
            Assert.Equal("<html><body>Ololo</body></html>", textBody.Text.Trim());
            Assert.True(textBody.IsHtml);
        }

        async Task<MimeMessage?> ReadLastMessageAsync()
        {
            using var client = new ImapClient();
            await client.ConnectAsync(ServerHost, 10163, false);

            await client.AuthenticateAsync(ServerLogin, ServerPassword);

            // The Inbox folder is always available on all IMAP servers...
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            MimeMessage? message  =null;

            if (inbox.Count != 0)
            {
                message = await inbox.GetMessageAsync(inbox.Count - 1);
            }

            await client.DisconnectAsync(true);

            return message;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            using var client = new ImapClient();
            await client.ConnectAsync(ServerHost, 10163, false);

            await client.AuthenticateAsync(ServerLogin, ServerPassword);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);

            var mailIndices = Enumerable.Repeat(0, inbox.Count)
                .Select((index, zero) => index)
                .ToList();

            await inbox.StoreAsync
                (
                    mailIndices, 
                    new StoreFlagsRequest
                        (
                            StoreAction.Add, 
                            MessageFlags.Deleted
                        )
                    {
                        Silent = true
                    });

            await inbox.ExpungeAsync();

        }
    }
}