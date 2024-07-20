using Moq;
using MyLab.EmailManager.App.Features.SendPendingMessages;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.MailServer;
using System.Linq.Expressions;
using System.Net.Mail;

namespace App.UnitTests
{
    public class SendPendingMessagesHandlerBehavior
    {
        Sending CreateSendingWithPendingMessage(string emailAddress, SendingStatus sendingStatus, SendingStatus msgStatus)
        {
            var msg = EmailMessage.New
            (
                Guid.NewGuid(),
                emailAddress,
                "bar",
                new TextContent("Simple content", false)
            );
            msg.SendingStatus = DatedValue<SendingStatus>.CreateSet(msgStatus);

            return new Sending
            (
                Guid.NewGuid(),
                new EmailLabel[]
                {
                    new("marker", "value")
                },
                "Simple content"
            )
            {
                SendingStatus = DatedValue<SendingStatus>.CreateSet(sendingStatus),
                Messages = new List<EmailMessage> { msg }
            };
        }

        [Fact]
        public async Task ShouldSendPendingOrSendingSendings()
        {
            //Arrange
            var pendingSending = CreateSendingWithPendingMessage
                (
                    "pending@host.com", 
                    sendingStatus: SendingStatus.Pending,
                    msgStatus: SendingStatus.Pending
                );
            var sendingSending = CreateSendingWithPendingMessage
                (
                    "sending@host.com", 
                    sendingStatus: SendingStatus.Sending,
                    msgStatus: SendingStatus.Pending
                );
            var sentSending = CreateSendingWithPendingMessage
                (
                    "sent@host.com", 
                    sendingStatus: SendingStatus.Sent, 
                    msgStatus: SendingStatus.Pending
                );

            var sendings = new List<Sending>
            {
                pendingSending,
                sendingSending,
                sentSending
            };

            var mailServerIntegration = new Mock<IMailServerIntegration>();

            var repo = new Mock<ISendingRepository>();
            repo.Setup
            (r => r.GetAsync
                (
                    It.IsAny<Expression<Func<Sending, bool>>>(), 
                    It.IsAny<CancellationToken>()
                )
            )
            .Returns<Expression<Func<Sending, bool>>, CancellationToken>
            ((spec, _) => Task.FromResult
                (
                    (IList<Sending>)sendings
                        .Where(spec.Compile())
                        .ToList()
                )
            );

            var handler = new SendPendingMessagesHandler(repo.Object, mailServerIntegration.Object);

            //Act
            await handler.Handle(new SendPendingMessagesCommand(), CancellationToken.None);

            //Assert
            mailServerIntegration.Verify
                (
                    i => i.SendMessageAsync
                        (
                            It.Is<string>(addr => addr == "pending@host.com"),
                            It.IsAny<string>(),
                            It.IsAny<TextContent>(),
                            It.IsAny<CancellationToken>()
                        ),
                    Times.Once
                );
            mailServerIntegration.Verify
            (
                i => i.SendMessageAsync
                (
                    It.Is<string>(addr => addr == "sending@host.com"),
                    It.IsAny<string>(),
                    It.IsAny<TextContent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
            mailServerIntegration.VerifyNoOtherCalls();
            Assert.Equal(SendingStatus.Sent, pendingSending.SendingStatus.Value);
            Assert.Equal(SendingStatus.Sent, pendingSending.Messages!.First().SendingStatus.Value);
            Assert.Equal(SendingStatus.Sent, sendingSending.SendingStatus.Value);
            Assert.Equal(SendingStatus.Sent, sendingSending.Messages!.First().SendingStatus.Value);
        }

        [Fact]
        public async Task ShouldSendOnlyPendingMessages()
        {
            //Arrange
            var pendingMsg = EmailMessage.New
            (
            Guid.NewGuid(),
                "pending@host.com",
                "bar",
                new TextContent("Simple content", false)
            );
            pendingMsg.SendingStatus = DatedValue<SendingStatus>.CreateSet(SendingStatus.Pending);

            var sendingMsg = EmailMessage.New
            (
                Guid.NewGuid(),
                "sent@host.com",
                "bar",
                new TextContent("Simple content", false)
            );
            sendingMsg.SendingStatus = DatedValue<SendingStatus>.CreateSet(SendingStatus.Sent);

            var sending = new Sending
            (
                Guid.NewGuid(),
                new EmailLabel[]
                {
                    new("marker", "value")
                },
                "Simple content"
            )
            {
                SendingStatus = DatedValue<SendingStatus>.CreateSet(SendingStatus.Sending),
                Messages = new List<EmailMessage>
                {
                    pendingMsg,
                    sendingMsg
                }
            };

            var mailServerIntegration = new Mock<IMailServerIntegration>();

            var repo = new Mock<ISendingRepository>();
            repo.Setup
            (r => r.GetAsync
                (
                    It.IsAny<Expression<Func<Sending, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .Returns<Expression<Func<Sending, bool>>, CancellationToken>
            ((spec, _) => Task.FromResult
                (
                    (IList<Sending>)new List<Sending>{ sending }
                )
            );

            var handler = new SendPendingMessagesHandler(repo.Object, mailServerIntegration.Object);

            //Act
            await handler.Handle(new SendPendingMessagesCommand(), CancellationToken.None);

            //Assert
            mailServerIntegration.Verify
                (
                    i => i.SendMessageAsync
                        (
                            It.Is<string>(addr => addr == "pending@host.com"),
                            It.IsAny<string>(),
                            It.IsAny<TextContent>(),
                            It.IsAny<CancellationToken>()
                        ),
                    Times.Once
                );
            mailServerIntegration.VerifyNoOtherCalls();
            Assert.Equal(SendingStatus.Sent, pendingMsg.SendingStatus.Value);
            Assert.Equal(SendingStatus.Sent, sending.SendingStatus.Value);
        }
    }
}
