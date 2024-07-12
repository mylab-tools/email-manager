using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLab.ApiClient.Test;
using MyLab.EmailManager;
using MyLab.EmailManager.App.Features.CreateOrUpdateEmail;
using MyLab.EmailManager.Client.Confirmations;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.WebErrors;
using System;
using Xunit.Abstractions;

namespace EmailManager.FuncTests
{
    public partial class ConfirmationsBehavior :
        IAsyncLifetime,
        IClassFixture<TestApiFixture<Program, IEmailManagerConfirmationsV1>>
    {
        //private readonly IEmailManagerConfirmationsV1 _client;
        private readonly TestDbFixture _dbFxt;
        //private readonly IServiceProvider _serviceProvider;
        //private readonly Guid _testEmailId = Guid.NewGuid();
        //private Guid _confirmationSeed;
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
                //var msgTemplateService = new Mock<IMessageTemplateService>();
                //msgTemplateService.Setup
                //(
                //    s => s.CreateTextContentAsync
                //    (
                //        It.Is<string>
                //        (
                //            str => str == ConfirmationMessageConstants.Confirmation
                //        ),
                //        It.IsAny<IDictionary<string, string>>()
                //    )
                //)
                //.ReturnsAsync<string, IDictionary<string, string>>((tId, args) => "Confirm");

                srv.Configure<ExceptionProcessingOptions>(opt => opt.HideError = false);
            };

            _apiFxt = apiFxt;

            //var proxyAsset = apiFxt.StartWithProxy();
            //_serviceProvider = proxyAsset.ServiceProvider;
            //_client = proxyAsset.ApiClient;
        }

        async Task<(Guid EmailId, Guid ConfirmationSeed)> CreateEmailAsync(IServiceProvider serviceProvider, string address = "foo@bar.com", IDictionary<string, string>? labels = null)
        {
            var scopedSp = serviceProvider
                .CreateScope()
                .ServiceProvider;

            var testEmailId = Guid.NewGuid();

            await scopedSp.GetRequiredService<IMediator>()
                .Send(new CreateOrUpdateEmailCommand(testEmailId, address, labels));

            var confirmationSeed = await scopedSp
                .GetRequiredService<ReadDbContext>()
                .Confirmations
                .Where(c => c.EmailId == testEmailId)
                .Select(c => c.Seed)
                .FirstAsync();

            return (testEmailId, confirmationSeed);
        }

        public async Task InitializeAsync()
        {
            //var scopedSp = _serviceProvider
            //    .CreateScope()
            //    .ServiceProvider;

            //await scopedSp.GetRequiredService<IMediator>()
            //    .Send(new CreateOrUpdateEmailCommand(_testEmailId, "foo@bar.com", null));

            //_confirmationSeed = await scopedSp
            //    .GetRequiredService<ReadDbContext>()
            //    .Confirmations
            //    .Where(c => c.EmailId == _testEmailId)
            //    .Select(c => c.Seed)
            //    .FirstAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbFxt.DisposeAsync();
        }
    }
}
