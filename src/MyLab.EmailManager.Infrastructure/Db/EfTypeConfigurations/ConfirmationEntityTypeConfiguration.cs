using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Infrastructure.Db.EfTypeConfigurations;

class ConfirmationEntityTypeConfiguration : IEntityTypeConfiguration<Confirmation>
{
    public void Configure(EntityTypeBuilder<Confirmation> builder)
    {
        builder.ToTable("confirmation")
            .HasKey(c => c.EmailId);
        builder.HasIndex(c => c.Seed);
        builder.Property(c => c.EmailId)
            .HasColumnName("email_id")
            .IsRequired();
        builder.OwnsOne(c => c.Step)
            .Property(d => d.Value)
            .HasColumnName("step");
        builder.Property(d => d.Seed)
            .IsRequired()
            .HasColumnName("seed");
        builder.OwnsOne(c => c.Step)
            .Property(d => d.DateTime)
            .HasColumnName("step_dt");
    }
}