using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class ConfirmationBehavior
    {
        private readonly Guid _testEmailId = Guid.NewGuid();
        
        [Fact]
        public void ShouldHasInitialStep()
        {
            //Arrange


            //Act
            var confirmation = new Confirmation(_testEmailId);

            //Assert
            Assert.Null(confirmation.Step.DateTime);
            Assert.Equal(ConfirmationStep.Undefined, confirmation.Step.Value);

        }

        [Fact]
        public void ShouldFailWhenWrongConfirmSeed()
        {
            //Arrange
            var confirmation = Confirmation.CreateNew(_testEmailId);

            //Act && Assert
            Assert.Throws<InvalidConfirmationSeedException>(() => confirmation.Complete(Guid.NewGuid()));
        }

        [Fact]
        public void ShouldPassWhenRightConfirmSeed()
        {
            //Arrange
            var confirmation = Confirmation.CreateNew(_testEmailId);

            //Act
            confirmation.Complete(confirmation.Seed);

            //Assert
            Assert.Equal(ConfirmationStep.Confirmed, confirmation.Step.Value);
        }

    }
}
