using System.Net;
using MyLab.ApiClient;
using MyLab.EmailManager.Client.Confirmations;

namespace EmailManager.FuncTests
{
    public partial class EmailManagerConfirmationsBehavior 
    {
        [Fact]
        public async Task ShouldGetState()
        {
            //Arrange

            //Act
            var confirmationState = await _client.GetStateAsync(_testEmailId);

            //Assert
            Assert.True(confirmationState is { Step: ConfirmationStateStep.Created, Confirmed:false });
        }

        [Fact]
        public async Task ShouldComplete()
        {
            //Arrange

            //Act
            await _client.CompleteAsync(_confirmationSeed);

            var confirmationState = await _client.GetStateAsync(_testEmailId);

            //Assert
            Assert.True(confirmationState is {Step:ConfirmationStateStep.Confirmed, Confirmed:true });
        }

        [Fact]
        public async Task ShouldNotCompleteWhenWrongSeed()
        {
            //Arrange
            var confirmationSeed = Guid.NewGuid();
            ResponseCodeException? rcE = null;

            //Act
            try
            {
                await _client.CompleteAsync(confirmationSeed);
            }
            catch (ResponseCodeException e)
            {
                rcE = e;
            }

            var confirmationState = await _client.GetStateAsync(_testEmailId);

            //Assert
            Assert.True(confirmationState is { Step: ConfirmationStateStep.Created, Confirmed: false});
            Assert.True(rcE is { StatusCode: HttpStatusCode.NotFound });
        }

        [Fact]
        public async Task ShouldRepeat()
        {
            //Arrange
            await _client.CompleteAsync(_confirmationSeed);

            //Act
            await _client.RepeatAsync(_testEmailId);

            var confirmationState = await _client.GetStateAsync(_testEmailId);

            //Assert
            Assert.True(confirmationState is { Step: ConfirmationStateStep.Created, Confirmed: false });
        }
    }
}
