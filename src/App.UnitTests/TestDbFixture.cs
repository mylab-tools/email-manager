using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using Xunit.Abstractions;

namespace App.UnitTests;

public class TestDbFixture : IAsyncDisposable
{
    private readonly string _filenameRandom;
    public DomainDbContext DomainDbContext { get; }
    public ReadDbContext ReadDbContext{ get; }
    public ITestOutputHelper? Output { get; set; }
    public TestDbFixture()
    {
        _filenameRandom = Guid.NewGuid().ToString("N");

        DbContextOptions<T> CreateTunedCtxBuilder<T>()
            where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .EnableSensitiveDataLogging()
                .UseSqlite($"Data Source=./{_filenameRandom}.db;")
                .LogTo(str => Output?.WriteLine(str), LogLevel.Information)
                .Options;
        }

        DomainDbContext = new DomainDbContext(CreateTunedCtxBuilder<DomainDbContext>());
        ReadDbContext = new ReadDbContext(CreateTunedCtxBuilder<ReadDbContext>());

        ReadDbContext.Database.EnsureCreated();
    }

    public async ValueTask DisposeAsync()
    {
        await ReadDbContext.Database.EnsureDeletedAsync();

        await DomainDbContext.DisposeAsync();
        await ReadDbContext.DisposeAsync();

        File.Delete($"./{_filenameRandom}.db");
    }
}