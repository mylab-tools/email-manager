using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.EmailManager.Client.Emails;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace EmailManager.FuncTests
{
    public class EmailManagerEmailsBehavior : 
        IAsyncLifetime,
        IClassFixture<TestApiFixture<Program, IEmailManagerEmailsV1>>
    {
        private readonly IEmailManagerEmailsV1 _client;
        private readonly DomainDbContext _dbContext;

        public EmailManagerEmailsBehavior(TestApiFixture<Program, IEmailManagerEmailsV1> fxt, ITestOutputHelper output)
        {
            fxt.Output = output;
            fxt.ServiceOverrider = srv =>
            {
                TestTools.SetupMemoryDb(srv);
                srv.Configure<ExceptionProcessingOptions>(opt => opt.HideError = false);
            };

            var proxyAsset = fxt.StartWithProxy();
            _client = proxyAsset.ApiClient;
            _dbContext = proxyAsset.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<DomainDbContext>();
        }

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

            //Assert
            Assert.NotNull(storedEmail);
            Assert.Equal(emailId, storedEmail.Id);
            Assert.Equal("foo@host.com", storedEmail.Address);
            Assert.NotNull(storedEmail.Labels);
            Assert.Single(storedEmail.Labels);
            Assert.Contains(storedEmail.Labels, kv => kv is { Key: "bar", Value: "baz"});
        }

        public async Task InitializeAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
        }
    }
}
