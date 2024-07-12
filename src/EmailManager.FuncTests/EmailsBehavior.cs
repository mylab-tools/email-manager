using Microsoft.Extensions.DependencyInjection;
using MyLab.EmailManager.Client.Emails;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;

namespace EmailManager.FuncTests
{
    public partial class EmailsBehavior
    {
        [Fact]
        public async Task ShouldCreateEmail()
        {
            //Arrange
            var emailDef = new EmailDefDto
            {
                Address = "foo@host.com",
                Labels = new Dictionary<string, string>
                {
                    { "bar", "baz" }
                }
            };


            //Act
            var emailId = await _client.CreateAsync(emailDef);

            var storedEmail = await _client.GetAsync(emailId);

            var confirmation = await _serviceProvider
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<IConfirmationRepository>()
                .GetAsync(emailId, CancellationToken.None);

            //Assert
            Assert.NotNull(storedEmail);
            Assert.Equal(emailId, storedEmail.Id);
            Assert.Equal("foo@host.com", storedEmail.Address);
            Assert.NotNull(storedEmail.Labels);
            Assert.Single(storedEmail.Labels);
            Assert.Contains(storedEmail.Labels, kv => kv is { Key: "bar", Value: "baz"});

            Assert.NotNull(confirmation);
            Assert.Equal(ConfirmationStep.Created, confirmation.Step.Value);
            Assert.NotEqual(Guid.Empty, confirmation.Seed);
        }

        [Fact]
        public async Task ShouldCreateEmailWithId()
        {
            //Arrange
            var emailDef = new EmailDefDto
            {
                Address = "foo@host.com",
                Labels = new Dictionary<string, string>
                {
                    { "bar", "baz" }
                }
            };
            var emailId = Guid.NewGuid();

            //Act
            await _client.CreateOrUpdateAsync(emailId, emailDef);
            var storedEmail = await _client.GetAsync(emailId);

            //Assert
            Assert.NotNull(storedEmail);
            Assert.Equal(emailId, storedEmail.Id);
            Assert.Equal("foo@host.com", storedEmail.Address);
            Assert.NotNull(storedEmail.Labels);
            Assert.Single(storedEmail.Labels);
            Assert.Contains(storedEmail.Labels, kv => kv is { Key: "bar", Value: "baz" });
        }

        [Fact]
        public async Task ShouldUpdateEmail()
        {
            //Arrange
            var emailDef = new EmailDefDto
            {
                Address = "foo@host.com",
                Labels = new Dictionary<string, string>
                {
                    { "bar", "baz" }
                }
            };

            var emailId = await _client.CreateAsync(emailDef);

            var changedEmail = new EmailDefDto
            {
                Address = "bar@host.com",
                Labels = new Dictionary<string, string>
                {
                    { "baz", "qoz" }
                }
            };

            //Act
            await _client.CreateOrUpdateAsync(emailId, changedEmail);
            var storedEmail = await _client.GetAsync(emailId);

            //Assert
            Assert.NotNull(storedEmail);
            Assert.Equal(emailId, storedEmail.Id);
            Assert.Equal("bar@host.com", storedEmail.Address);
            Assert.NotNull(storedEmail.Labels);
            Assert.Single(storedEmail.Labels);
            Assert.Contains(storedEmail.Labels, kv => kv is { Key: "baz", Value: "qoz" });
        }

        [Fact]
        public async Task ShouldDeleteEmail()
        {
            //Arrange
            var emailDef = new EmailDefDto
            {
                Address = "foo@host.com",
                Labels = new Dictionary<string, string>
                {
                    { "bar", "baz" }
                }
            };

            var emailId = await _client.CreateAsync(emailDef);

            //Act
            await _client.DeleteAsync(emailId);
            var storedEmail = await _client.GetAsync(emailId);

            //Assert
            Assert.Null(storedEmail);
        }
    }
}
