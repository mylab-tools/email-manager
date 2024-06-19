using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure
{
    public class EmailDbContext : DbContext
    {
        public DbSet<Email> Emails => Set<Email>();
        public DbSet<EmailLabel> EmailLabels => Set<EmailLabel>();

        public EmailDbContext(DbContextOptions<EmailDbContext> opts) : base(opts)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmailLabelEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
