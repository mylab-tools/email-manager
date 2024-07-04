
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Infrastructure;

namespace Migrations
{
    public class MigrationDbContext : DomainDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
            optionsBuilder.UseMySql("Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", serverVersion);
            //optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=db;Uid=user;Pwd=password;", serverVersion);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
