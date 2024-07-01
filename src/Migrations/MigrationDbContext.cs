
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Infrastructure;

namespace Migrations
{
    public class MigrationDbContext : AppDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
            optionsBuilder.UseMySql("Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", serverVersion);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
