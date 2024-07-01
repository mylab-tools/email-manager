using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure
{
    public class AppDbContext: DbContext
    {
        public DbSet<Email> Emails => Set<Email>();
        public DbSet<EmailLabel> EmailLabels => Set<EmailLabel>();
        public DbSet<Confirmation> Confirmations => Set<Confirmation>();
        public DbSet<Sending> Sendings => Set<Sending>();

        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts){}
        protected AppDbContext(){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmailLabelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ConfirmationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SendingEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
