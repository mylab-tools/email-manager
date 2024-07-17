using Moq;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace Infrastructure.UnitTests
{
    public class MessageTemplateServiceBehavior
    {
        [Fact]
        public async Task ShouldRenderTextContent()
        {
            //Arrange
            var tProvider = new Mock<IMessageTemplateProvider>();
            tProvider.Setup(p => p.ProvideAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new TextContent("{{args.action}}, {{email.owner}}!", false));

            var srv = new MessageTemplateService(tProvider.Object);

            //Act
            var content = await srv.CreateTextContentAsync
                (
                    "foo",
                    new TemplateContext
                        (
                            new Dictionary<string, string>
                            {
                                {"action", "Let's go"}
                            },
                            new Dictionary<string, string>
                            {
                                {"owner", "Brandon"}
                            }
                        ),
                    CancellationToken.None
                );

            //Assert
            Assert.Equal("Let's go, Brandon!", content.Text);
            Assert.False(content.IsHtml);
        }
    }
}
