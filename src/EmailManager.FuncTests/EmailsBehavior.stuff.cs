using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.EmailManager.Client.Emails;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace EmailManager.FuncTests;

public partial class EmailsBehavior :
    IAsyncLifetime,
    IClassFixture<TestApiFixture<Program, IEmailManagerEmailsV1>>
{
    private readonly IEmailManagerEmailsV1 _client;
    private readonly TestDbFixture _dbFxt;
    private readonly IServiceProvider _serviceProvider;

    public EmailsBehavior
    (
        TestApiFixture<Program, IEmailManagerEmailsV1> apiFxt,
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

        var proxyAsset = apiFxt.StartWithProxy();
        _serviceProvider = proxyAsset.ServiceProvider;
        _client = proxyAsset.ApiClient;
    }

    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        await _dbFxt.DisposeAsync();
    }
}