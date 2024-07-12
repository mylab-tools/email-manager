using Moq;
using MyLab.EmailManager.Infrastructure.MessageTemplates;

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
                .ReturnsAsync(() => "Hellow, {{bar}}!");

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
            Assert.Equal("Hellow, world!", text);
        }
    }
}
