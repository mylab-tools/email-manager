using Moq;
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
            tProvider.Setup(p => p.ProvideAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new TextContent("Hellow, {{bar}}!", false));

            var srv = new MessageTemplateService(tProvider.Object);

            //Act
            var text = await srv.CreateTextContentAsync
                (
                    "foo",
                    new Dictionary<string, string>
                    {
                        {"bar", "world"}
                    }
                );

            //Assert
            Assert.Equal("Hellow, world!", text.Content);
            Assert.False(text.IsHtml);
        }
    }
}
