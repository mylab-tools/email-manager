using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.App.Features.CreateSending;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using System.Linq;
using Xunit.Abstractions;

namespace App.UnitTests
{
    public class WhereDbLabelBehavior : IAsyncLifetime
    {
        private readonly TestDbFixture _dbFxt;
        private readonly ReadDbContext _db;

        private readonly Guid _redHighPriorityEmailId = Guid.NewGuid();
        private readonly Guid _redLowPriorityEmailId = Guid.NewGuid();
        private readonly Guid _greenLowPriorityEmailId = Guid.NewGuid();
        private readonly Guid _greenHighPriorityEmailId = Guid.NewGuid();

        public WhereDbLabelBehavior(ITestOutputHelper output)
        {
            _dbFxt = new TestDbFixture
            {
                Output = output
            };
            _db = _dbFxt.ReadDbContext;
        }

        [Fact]
        public async Task ShouldSelectWithLabel()
        {
            //Arrange
            var kv = new KeyValuePair<string, string>("color", "red");

            //Act
            var found = await _db.Labels
                .Where(WhereDbLabel.EqualsTo(kv))
                .Select(l => l.EmailId)
                .Distinct()
                .ToArrayAsync();

            //Assert
            Assert.Equal(2, found.Length);
            Assert.Contains(found, eId => eId == _redHighPriorityEmailId);
            Assert.Contains(found, eId => eId == _redLowPriorityEmailId);
        }

        [Fact]
        public async Task ShouldSelectWithSingleItemLabelCollection()
        {
            //Arrange
            var dict = new Dictionary<string, string>
            {
                {"color", "red"},
            }.AsReadOnly();

            //Act
            var found = await _db.Labels
                .Where(WhereDbLabel.In(dict))
                .Select(l => l.EmailId)
                .Distinct()
                .ToArrayAsync();

            //Assert
            Assert.Equal(2, found.Length);
            Assert.Contains(found, eId => eId == _redHighPriorityEmailId);
            Assert.Contains(found, eId => eId == _redLowPriorityEmailId);
        }

        [Fact]
        public async Task ShouldSelectWithLabelCollection()
        {
            //Arrange
            var dict = new Dictionary<string, string>
            {
                {"color", "red"},
                {"priority", "low"}
            }.AsReadOnly();

            //Act
            var found = await _db.Labels
                .Where(WhereDbLabel.In(dict))
                .Select(l => l.EmailId)
                .Distinct()
                .ToArrayAsync();

            //Assert
            Assert.Equal(3, found.Length);
            Assert.Contains(found, eId => eId == _redHighPriorityEmailId);
            Assert.Contains(found, eId => eId == _redLowPriorityEmailId);
            Assert.Contains(found, eId => eId == _greenLowPriorityEmailId);
        }

        public async Task InitializeAsync()
        {
            await _db.Emails.AddAsync
            (
                new DbEmail
                {
                    Id = _redHighPriorityEmailId,
                    Address = "foo@host.com",
                    Labels = new List<DbLabel>
                    {
                        new() { Name = "color", Value = "red" },
                        new() { Name = "priority", Value = "high" },
                    }
                }
            );

            await _db.Emails.AddAsync
            (
                new DbEmail
                {
                    Id = _redLowPriorityEmailId,
                    Address = "bar@host.com",
                    Labels = new List<DbLabel>
                    {
                        new() { Name = "color", Value = "red" },
                        new() { Name = "priority", Value = "low" },
                    }
                }
            );

            await _db.Emails.AddAsync
            (
                new DbEmail
                {
                    Id = _greenLowPriorityEmailId,
                    Address = "baz@host.com",
                    Labels = new List<DbLabel>
                    {
                        new() { Name = "color", Value = "green" },
                        new() { Name = "priority", Value = "low" },
                    }
                }
            );

            await _db.Emails.AddAsync
            (
                new DbEmail
                {
                    Id = _greenHighPriorityEmailId,
                    Address = "qoz@host.com",
                    Labels = new List<DbLabel>
                    {
                        new() { Name = "color", Value = "green" },
                        new() { Name = "priority", Value = "high" },
                    }
                }
            );

            await _db.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbFxt.DisposeAsync();
        }
    }
}