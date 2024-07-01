using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Infrastructure.EfTypeConfigurations;

class ConfirmationEntityTypeConfiguration : IEntityTypeConfiguration<Confirmation>
{
    public void Configure(EntityTypeBuilder<Confirmation> builder)
    {
        builder.ToTable("confirmation")
            .HasKey(c => c.EmailId);
        builder.Property(c => c.EmailId)
            .HasColumnName("email_id")
            .IsRequired();
        builder.OwnsOne(c => c.Step)
            .Property(d => d.Value)
            .HasColumnName("step");
        builder.OwnsOne(c => c.Step)
            .Property(d => d.DateTime)
            .HasColumnName("step_dt");
        builder.HasOne<Email>()
            .WithOne()
            .HasForeignKey<Confirmation>(c => c.EmailId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}