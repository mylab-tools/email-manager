using MyLab.EmailManager.Domain.Dto;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class SendingBehavior
    {
        [Theory]
        [MemberData(nameof(GetWrongMessageContentCases))]
        public void ShouldFailIfWrongMessageContent(SimpleMessageContent simpleMessageContent, GenericMessageContent genericMessageContent)
        {
            //Arrange
            var message = new SendMessageDef
            {
                Title = "foo",
                Selection = new EmailLabel[] { new("bar", "baz") },
                SimpleContent = simpleMessageContent,
                GenericContent = genericMessageContent
            };

            //Act & Assert
            Assert.Throws<DomainException>(() => new Sending(Guid.NewGuid(), message));
        }

        public static object[][] GetWrongMessageContentCases()
        {
            return new object[][]
            {
                new object[] { null, null },
                new object[]
                {
                    new SimpleMessageContent("foo"),
                    new GenericMessageContent
                    (
                        "bar",
                        new Dictionary<FilledString, string>
                        {
                            { "baz", "qoz" }
                        }
                    )
                }
            };
        }
    }
}
