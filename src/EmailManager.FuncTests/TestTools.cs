using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace EmailManager.FuncTests
{
    static class TestTools
    {
        public static void SetupMemoryDb(IServiceCollection srv)
        {
            var domainDbContextSrvDesc = srv.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<DomainDbContext>));
            if (domainDbContextSrvDesc != null)
                srv.Remove(domainDbContextSrvDesc);

            var readDbContextSrvDesc = srv.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<ReadDbContext>));
            if (readDbContextSrvDesc != null)
                srv.Remove(readDbContextSrvDesc);

            Action<DbContextOptionsBuilder> tuneDbOpts = opt => opt
                .UseSqlite("Data Source=:memory:");

            srv.AddDbContext<DomainDbContext>(tuneDbOpts);
            srv.AddDbContext<ReadDbContext>(tuneDbOpts);
        }
    }
}
