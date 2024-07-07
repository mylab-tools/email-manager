using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace EmailManager.FuncTests
{
    static class TestTools
    {
        private static readonly InMemoryDatabaseRoot DbRoot = new();

        public static void SetupMemoryDb(IServiceCollection srv, DomainDbContext domainDbContext, ReadDbContext readDbContext)
        {   
            srv.AddSingleton(domainDbContext);
            srv.AddSingleton(readDbContext);
        }
    }
}
