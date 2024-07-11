using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.EmailManager.App.Features.CreateEmail;
using MyLab.EmailManager.App.Features.CreateOrUpdateEmail;
using MyLab.EmailManager.Client.Confirmations;
using MyLab.EmailManager.Client.Emails;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace EmailManager.FuncTests
{
    public partial class EmailManagerConfirmationsBehavior :
        IAsyncLifetime,
        IClassFixture<TestApiFixture<Program, IEmailManagerConfirmationsV1>>
    {
        private readonly IEmailManagerConfirmationsV1 _client;
        private readonly TestDbFixture _dbFxt;
        private readonly IServiceProvider _serviceProvider;
        private readonly Guid _testEmailId = Guid.NewGuid();
        private Guid _confirmationSeed;


        public EmailManagerConfirmationsBehavior
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
                srv.Configure<ExceptionProcessingOptions>(opt => opt.HideError = false);
            };

            var proxyAsset = apiFxt.StartWithProxy();
            _serviceProvider = proxyAsset.ServiceProvider;
            _client = proxyAsset.ApiClient;
        }

        public async Task InitializeAsync()
        {
            var scopedSp = _serviceProvider
                .CreateScope()
                .ServiceProvider;

            await scopedSp.GetRequiredService<IMediator>()
                .Send(new CreateOrUpdateEmailCommand(_testEmailId, "foo@bar.com", null));

            _confirmationSeed = await scopedSp
                .GetRequiredService<ReadDbContext>()
                .Confirmations
                .Where(c => c.EmailId == _testEmailId)
                .Select(c => c.Seed)
                .FirstAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbFxt.DisposeAsync();
        }
    }
}
