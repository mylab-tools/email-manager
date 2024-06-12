using System.ComponentModel.DataAnnotations;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class EmailAddressBehavior
    {
        [Theory]
        [InlineData("username@host.com", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("   ", false)]
        [InlineData("foo", false)]
        [InlineData("foo@bar", false)]
        public void ShouldValidateAddress(string addressString, bool valid)
        {
            //Arrange
            ValidationException? validationException = null;

            //Act
            try
            {
                EmailAddress.Validate(addressString);
            }
            catch (ValidationException e)
            {
                validationException = e;
            }

            //Assert
            Assert.Equal(valid, validationException == null);

        }

        [Fact]
        public void ShouldImplicitCastFromString()
        {
            //Arrange
            const string addrValue = "login@host.com";

            //Act
            EmailAddress addr = addrValue;

            //Assert
            Assert.Equal(addrValue, addr.Address);

        }
    }
}