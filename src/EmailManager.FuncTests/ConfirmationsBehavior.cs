using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLab.ApiClient;
using MyLab.EmailManager.App.ConfirmationStuff;
using MyLab.EmailManager.Client.Confirmations;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.MailServer;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace EmailManager.FuncTests
{
    using static TestTools;

    public partial class ConfirmationsBehavior 
    {
        [Fact]
        public async Task ShouldGetState()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var createdEmail = await CreateEmailAsync(clientAsset.ServiceProvider);

            //Act
            var confirmationState = await clientAsset.ApiClient.GetStateAsync(createdEmail.EmailId);

            //Assert
            Assert.True(confirmationState is { Step: ConfirmationStateStep.Created, Confirmed:false });
        }

        [Fact]
        public async Task ShouldComplete()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var createdEmail = await CreateEmailAsync(clientAsset.ServiceProvider);

            //Act
            await clientAsset.ApiClient.CompleteAsync(createdEmail.ConfirmationSeed);

            var confirmationState = await clientAsset.ApiClient.GetStateAsync(createdEmail.EmailId);

            //Assert
            Assert.True(confirmationState is {Step:ConfirmationStateStep.Confirmed, Confirmed:true });
        }

        [Fact]
        public async Task ShouldNotCompleteWhenWrongSeed()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var createdEmail = await CreateEmailAsync(clientAsset.ServiceProvider);

            var confirmationSeed = Guid.NewGuid();
            ResponseCodeException? rcE = null;

            //Act
            try
            {
                await clientAsset.ApiClient.CompleteAsync(confirmationSeed);
            }
            catch (ResponseCodeException e)
            {
                rcE = e;
            }

            var confirmationState = await clientAsset.ApiClient.GetStateAsync(createdEmail.EmailId);

            //Assert
            Assert.True(confirmationState is { Step: ConfirmationStateStep.Created, Confirmed: false});
            Assert.True(rcE is { StatusCode: HttpStatusCode.NotFound });
        }

        [Fact]
        public async Task ShouldRepeat()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var createdEmail = await CreateEmailAsync(clientAsset.ServiceProvider);

            await clientAsset.ApiClient.CompleteAsync(createdEmail.ConfirmationSeed);

            //Act
            await clientAsset.ApiClient.RepeatAsync(createdEmail.EmailId);

            var confirmationState = await clientAsset.ApiClient.GetStateAsync(createdEmail.EmailId);

            //Assert
            Assert.True(confirmationState is { Step: ConfirmationStateStep.Created, Confirmed: false });
        }
        
        [Fact]
        public async Task ShouldSendConfirmationMessage()
        {
            //Arrange
            var msgTemplateProvider = new Mock<IMessageTemplateProvider>();
            msgTemplateProvider.Setup
            (
                s => s.ProvideAsync
                (
                    It.Is<string>
                    (
                        str => str == ConfirmationMessageConstants.TemplateId
                    ),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(() => new TextContent("Hello, {{args.user}}! Confirm email please!", false));

            string? sentMsgAddr = null;
            string? sentMsgSubject = null;
            TextContent? sentMsgContent = null;

            var mailIntegration = new Mock<IMailServerIntegration>();
            mailIntegration.Setup
            (
                i => i.SendMessageAsync
                (
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<TextContent>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .Callback<string, string, TextContent, CancellationToken>((addr, sub, content, _) =>
            {
                sentMsgAddr = addr;
                sentMsgSubject = sub;
                sentMsgContent = content;
            });

            var clientAsset = _apiFxt.StartWithProxy(srv =>
            {
                srv.AddSingleton(msgTemplateProvider.Object);
                srv.AddSingleton(mailIntegration.Object);
                srv.AddSingleton<IMailMessageSender, MailMessageSender>();
            });

            //Act
            await CreateEmailAsync
                (
                    clientAsset.ServiceProvider,
                    address: "foo@bar.com",
                    labels: new Dictionary<string, string>
                    {
                        { "user", "Mr.Freeman"}
                    }
                );

            //Assert
            Assert.Equal("foo@bar.com", sentMsgAddr);
            Assert.Equal(ConfirmationMessageConstants.DefaultSubject, sentMsgSubject);
            Assert.NotNull(sentMsgContent);
            Assert.False(sentMsgContent.IsHtml);
            Assert.Equal("Hello, Mr.Freeman! Confirm email please!", sentMsgContent.Text);
        }

        [Fact]
        public async Task ShouldSendConfirmationMessageWhenRepeat()
        {
            //Arrange
            var msgTemplateProvider = new Mock<IMessageTemplateProvider>();
            msgTemplateProvider.Setup
                (
                    s => s.ProvideAsync
                    (
                        It.Is<string>
                        (
                            str => str == ConfirmationMessageConstants.TemplateId
                        ),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ReturnsAsync(() => new TextContent("Hello, {{args.user}}! Confirm email please!", false));

            string? sentMsgAddr = null;
            string? sentMsgSubject = null;
            TextContent? sentMsgContent = null;

            bool confMailCatching = false;

            var mailIntegration = new Mock<IMailServerIntegration>();
            mailIntegration.Setup
                (
                    i => i.SendMessageAsync
                    (
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<TextContent>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .Callback<string, string, TextContent, CancellationToken>((addr, sub, content, _) =>
                {
                    if (confMailCatching)
                    {
                        sentMsgAddr = addr;
                        sentMsgSubject = sub;
                        sentMsgContent = content;
                    }
                });

            var clientAsset = _apiFxt.StartWithProxy(srv =>
            {
                srv.AddSingleton(msgTemplateProvider.Object);
                srv.AddSingleton(mailIntegration.Object);
                srv.AddSingleton<IMailMessageSender, MailMessageSender>();
            });

            var createdEmail = await CreateEmailAsync
            (
                clientAsset.ServiceProvider,
                address: "foo@bar.com",
                labels: new Dictionary<string, string>
                {
                    { "user", "Mr.Freeman"}
                }
            );

            confMailCatching = true;

            //Act
            await clientAsset.ApiClient.RepeatAsync(createdEmail.EmailId);

            //Assert
            Assert.Equal("foo@bar.com", sentMsgAddr);
            Assert.Equal(ConfirmationMessageConstants.DefaultSubject, sentMsgSubject);
            Assert.NotNull(sentMsgContent);
            Assert.False(sentMsgContent.IsHtml);
            Assert.Equal("Hello, Mr.Freeman! Confirm email please!", sentMsgContent.Text);
        }

    }
}
