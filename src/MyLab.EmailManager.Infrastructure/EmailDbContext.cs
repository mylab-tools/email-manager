using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure
{
    public class EmailDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmailLabelTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
}
