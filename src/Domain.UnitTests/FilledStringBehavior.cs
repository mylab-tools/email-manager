using System.ComponentModel.DataAnnotations;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Domain.UnitTests
{
    public class FilledStringBehavior
    {
        [Theory]
        [InlineData("foo", true)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("           ", false)]
        [InlineData(null, false)]
        public void ShouldValidateValue(string value, bool valid)
        {
            //Arrange
            DomainValidationException? validationException = null;

            //Act
            try
            {
                FilledString.Validate(value);
            }
            catch (DomainValidationException e)
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
            const string value = "foo";

            //Act
            FilledString fs = value;

            //Assert
            Assert.Equal(value, fs.Value);

        }
    }
}
