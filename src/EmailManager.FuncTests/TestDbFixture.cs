using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace EmailManager.FuncTests;

public class TestDbFixture : IAsyncDisposable
{
    private readonly string _filenameRandom;
    public DomainDbContext DomainDbContext { get; }
    public ReadDbContext ReadDbContext{ get; }
    public TestDbFixture()
    {
        _filenameRandom = Guid.NewGuid().ToString("N");

        DbContextOptions<T> CreateTunedCtxBuilder<T>()
            where T : DbContext
        {
            return new DbContextOptionsBuilder<T>().UseSqlite($"Data Source=./{_filenameRandom}.db;").Options;
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