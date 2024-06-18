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
        public void ShouldFailIfWrongMessageContent(SimpleMessageDef simpleMessageContent, GenericMessageDef genericMessageContent)
        {
            //Arrange
            var message = new SendMessageDef
            {
                Title = "foo",
                Selection = new EmailLabel[] { new("bar", "baz") },
                SimpleMsg = simpleMessageContent,
                GenericMsg = genericMessageContent
            };

            //Act & Assert
            Assert.Throws<DomainException>(() => Sending.Create(message));
        }

        public static object[][] GetWrongMessageContentCases()
        {
            return new object[][]
            {
                new object[] { null, null },
                new object[]
                {
                    new SimpleMessageDef("foo"),
                    new GenericMessageDef
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
