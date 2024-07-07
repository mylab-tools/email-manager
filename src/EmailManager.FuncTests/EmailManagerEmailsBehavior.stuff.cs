using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.EmailManager.Client.Emails;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace EmailManager.FuncTests;

public partial class EmailManagerEmailsBehavior :
    IAsyncLifetime,
    IClassFixture<TestApiFixture<Program, IEmailManagerEmailsV1>>
{
    private readonly IEmailManagerEmailsV1 _client;
    private readonly TestDbFixture _dbFxt;

    public EmailManagerEmailsBehavior
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
            srv.Configure<ExceptionProcessingOptions>(opt => opt.HideError = false);
        };

        var proxyAsset = apiFxt.StartWithProxy();
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