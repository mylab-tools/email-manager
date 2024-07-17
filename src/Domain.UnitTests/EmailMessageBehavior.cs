using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class EmailMessageBehavior
    {
        private readonly EmailMessage _testMessage = EmailMessage.New
        (
            Guid.NewGuid(),
            "foo@host.com",
            "Hello!",
            new TextContent("Hello", false)
        );

        [Fact]
        public void ShouldSetCurrentDateTimeAsCreateDtWhenJustCreated()
        {
            //Arrange

            //Act

            //Assert
            Assert.True(DateTime.Now.AddSeconds(-1) < _testMessage.CreateDt && _testMessage.CreateDt < DateTime.Now.AddSeconds(1));
        }

        [Fact]
        public void ShouldNotSetSendDtWhenJustCreated()
        {
            //Arrange

            //Act

            //Assert
            Assert.False(_testMessage.SendDt.HasValue);
        }

        [Fact]
        public void ShouldSetSendDtIfNotDefinedBefore()
        {
            //Arrange
            
            //Act
            _testMessage.SetCurrentSendDateTime();

            //Assert
            Assert.True(DateTime.Now.AddSeconds(-1) < _testMessage.SendDt && _testMessage.SendDt < DateTime.Now.AddSeconds(1));
        }

        [Fact]
        public void ShouldNotSetSendDtIfDefinedBefore()
        {
            //Arrange
            _testMessage.SetCurrentSendDateTime();

            //Act & Assert
            Assert.Throws<DomainException>(_testMessage.SetCurrentSendDateTime);
        }
    }
}
