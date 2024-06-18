using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class EmailBehavior
    {
        private const string TestEmailAddress = "login@host.com";

        [Fact]
        public void ShouldUpdateLabels()
        {
            //Arrange
            var email = new Email(TestEmailAddress);
            email.Labels.Add(new EmailLabel("foo", "bar"));

            //Act
            email.UpdateLabels(new []{ new EmailLabel("baz", "qoz") });

            //Assert
            Assert.Single(email.Labels);
            Assert.Contains(email.Labels, l => l.Name.Text == "baz" && l.Value == "qoz");

        }

        [Fact]
        public void ShouldHasInitialDeletionInfo()
        {
            //Arrange


            //Act
            var email = new Email(TestEmailAddress);

            //Assert
            Assert.Null(email.Deletion.DateTime);
            Assert.False(email.Deletion.Value);

        }

        [Fact]
        public void ShouldSafeDelete()
        {
            //Arrange
            var email = new Email(TestEmailAddress);

            //Act
            email.Delete();

            //Assert
            Assert.True(email.Deletion.Value);
            Assert.NotNull(email.Deletion.DateTime);
        }

        [Fact]
        public void ShouldFailIfTryToDeleteAlreadyDeleted()
        {
            //Arrange
            var email = new Email(TestEmailAddress);
            email.Delete();

            DomainException? domainException = null;

            //Act
            try
            {
                email.Delete();
            }
            catch (DomainException e)
            {
                domainException = e;
            }

            //Assert
            Assert.NotNull(domainException);
        }
    }
}
