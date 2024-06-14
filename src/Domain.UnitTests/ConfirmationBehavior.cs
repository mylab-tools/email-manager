using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class ConfirmationBehavior
    {
        private readonly Guid _testEmailId = Guid.NewGuid();

        [Fact]
        public void ShouldHasInitialDeletionInfo()
        {
            //Arrange


            //Act
            var confirmation = new Confirmation(_testEmailId);

            //Assert
            Assert.Null(confirmation.Deletion.DateTime);
            Assert.False(confirmation.Deletion.Value);

        }

        [Fact]
        public void ShouldSafeDelete()
        {
            //Arrange
            var confirmation = new Confirmation(_testEmailId);

            //Act
            confirmation.Delete();

            //Assert
            Assert.True(confirmation.Deletion.Value);
            Assert.NotNull(confirmation.Deletion.DateTime);
        }

        [Fact]
        public void ShouldFailIfTryToDeleteAlreadyDeleted()
        {
            //Arrange
            var confirmation = new Confirmation(_testEmailId);
            confirmation.Delete();

            DomainException? domainException = null;

            //Act
            try
            {
                confirmation.Delete();
            }
            catch (DomainException e)
            {
                domainException = e;
            }

            //Assert
            Assert.NotNull(domainException);
        }

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

        [Theory]
        [InlineData(ConfirmationStep.Undefined, ConfirmationStep.Created)]
        [InlineData(ConfirmationStep.Undefined, ConfirmationStep.Sent)]
        [InlineData(ConfirmationStep.Undefined, ConfirmationStep.Confirmed)]
        [InlineData(ConfirmationStep.Created, ConfirmationStep.Sent)]
        [InlineData(ConfirmationStep.Created, ConfirmationStep.Confirmed)]
        [InlineData(ConfirmationStep.Sent, ConfirmationStep.Confirmed)]
        public void ShouldChangeStepToOneOfNext(ConfirmationStep initialStep, ConfirmationStep newStep)
        {
            //Arrange
            var confirmation = new Confirmation(_testEmailId, initialStep);

            //Act & Assert
            confirmation.ChangeStep(newStep);
        }

        [Theory]
        [InlineData(ConfirmationStep.Undefined, ConfirmationStep.Undefined)]
        [InlineData(ConfirmationStep.Created, ConfirmationStep.Undefined)]
        [InlineData(ConfirmationStep.Created, ConfirmationStep.Created)]
        [InlineData(ConfirmationStep.Sent, ConfirmationStep.Undefined)]
        [InlineData(ConfirmationStep.Sent, ConfirmationStep.Created)]
        [InlineData(ConfirmationStep.Sent, ConfirmationStep.Sent)]
        [InlineData(ConfirmationStep.Confirmed, ConfirmationStep.Undefined)]
        [InlineData(ConfirmationStep.Confirmed, ConfirmationStep.Created)]
        [InlineData(ConfirmationStep.Confirmed, ConfirmationStep.Sent)]
        [InlineData(ConfirmationStep.Confirmed, ConfirmationStep.Confirmed)]
        public void ShouldFailIfChangeToNotNext(ConfirmationStep initialStep, ConfirmationStep newStep)
        {
            //Arrange
            var confirmation = new Confirmation(_testEmailId, initialStep);

            //Act & Assert
            Assert.Throws<InvalidNewConfirmationStepException>(() => confirmation.ChangeStep(newStep));
        }

    }
}
