using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.EmailManager.Client.Confirmations;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace EmailManager.FuncTests
{
    public partial class ConfirmationsBehavior :
        IAsyncLifetime,
        IClassFixture<TestApiFixture<Program, IEmailManagerConfirmationsV1>>
    {
        private readonly TestDbFixture _dbFxt;
        private readonly TestApiFixture<Program, IEmailManagerConfirmationsV1> _apiFxt;

        public ConfirmationsBehavior
        (
            TestApiFixture<Program, IEmailManagerConfirmationsV1> apiFxt,
            ITestOutputHelper output
        )
        {
            _dbFxt = new TestDbFixture();
            apiFxt.Output = output;
            apiFxt.ServiceOverrider = srv =>
            {
                TestTools.SetupMemoryDb(srv, _dbFxt.DomainDbContext, _dbFxt.ReadDbContext);
                TestTools.SetFakeConfiguration(srv);
                TestTools.RegisterSilentMailStuff(srv);

                srv.Configure<ExceptionProcessingOptions>(opt => opt.HideError = false);
            };

            _apiFxt = apiFxt;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _dbFxt.DisposeAsync();
        }
    }
}
