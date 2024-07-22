using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db.EfTypeConfigurations;

namespace MyLab.EmailManager.Infrastructure.Db
{
    public class DomainDbContext : DbContext
    {
        public DbSet<Email> Emails => Set<Email>();
        public DbSet<EmailLabel> EmailLabels => Set<EmailLabel>();
        public DbSet<Confirmation> Confirmations => Set<Confirmation>();
        public DbSet<Sending> Sendings => Set<Sending>();

        public DomainDbContext(DbContextOptions<DomainDbContext> opts) : base(opts) { }
        protected DomainDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmailLabelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ConfirmationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SendingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmailMessageEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
