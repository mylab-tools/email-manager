using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.EmailManager.App.Features.CreateOrUpdateEmail;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Client.Sendings;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace EmailManager.FuncTests
{
    using static TestTools;
    public partial class SendingsBehavior :
        IAsyncLifetime,
        IClassFixture<TestApiFixture<Program, IEmailManagerSendingsV1>>
    {
        private readonly TestDbFixture _dbFxt;
        private readonly TestApiFixture<Program, IEmailManagerSendingsV1> _apiFxt;

        public SendingsBehavior
        (
            TestApiFixture<Program, IEmailManagerSendingsV1> apiFxt,
            ITestOutputHelper output
        )
        {
            _dbFxt = new TestDbFixture();
            apiFxt.Output = output;
            apiFxt.ServiceOverrider = srv =>
            {
                SetupMemoryDb(srv, _dbFxt.DomainDbContext, _dbFxt.ReadDbContext);
                SetFakeConfiguration(srv);
                RegisterSilentMailStuff(srv);

                srv.Configure<ExceptionProcessingOptions>(opt => opt.HideError = false);
            };

            _apiFxt = apiFxt;
        }

        private async Task<Guid> CreateAndConfirmEmail(IServiceProvider sp,  string marker1, string marker2)
        {
            var createdEmail = await CreateEmailAsync
            (
                sp,
                labels: new Dictionary<string, string>
                {
                    {"marker1", marker1},
                    {"marker2", marker2}
                }
            );

            await ConfirmEmailAsync(sp, createdEmail.ConfirmationSeed);

            return createdEmail.EmailId;
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
