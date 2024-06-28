using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Infrastructure;

namespace Migrations
{
    public class MigrationDbContext(DbContextOptions<AppDbContext> opts) : AppDbContext(opts)
    {

    }
}
