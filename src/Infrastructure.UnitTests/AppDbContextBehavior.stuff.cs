using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Infrastructure;
using Xunit.Abstractions;

namespace Infrastructure.UnitTests;

public partial class AppDbContextBehavior
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _dbContext;

    public AppDbContextBehavior(ITestOutputHelper output)
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .LogTo((_, lvl) => lvl >= LogLevel.Information, d =>
            {
                output.WriteLine(d.ToString());
                output.WriteLine("");
            })
            .Options;

        _dbContext = new AppDbContext(contextOptions);
    }

    public Task InitializeAsync()
    {
        return _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    Email SaveTestEmail()
    {
        var actualEmail = new Email(Guid.NewGuid(), "foo@host.com");

        _dbContext.Emails.Add(actualEmail);
        _dbContext.SaveChanges();

        return actualEmail;
    }
}