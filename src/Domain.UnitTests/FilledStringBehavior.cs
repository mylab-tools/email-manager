using System.ComponentModel.DataAnnotations;
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
            ValidationException? validationException = null;

            //Act
            try
            {
                FilledString.Validate(value);
            }
            catch (ValidationException e)
            {
                validationException = e;
            }

            //Assert
            Assert.Equal(valid, validationException == null);

        }
    }
}
