using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db.EfConverters;

namespace MyLab.EmailManager.Infrastructure.Db.EfTypeConfigurations;

internal class EmailEntityTypeConfiguration : IEntityTypeConfiguration<Email>
{
    public const string EmailLabelToEmailFkFieldName = "email_id";

    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("email")
            .HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();
        builder.OwnsOne(e => e.Deletion)
            .Property(d => d.Value)
            .HasColumnName("deleted");
        builder.OwnsOne(e => e.Deletion)
            .Property(d => d.DateTime)
            .HasColumnName("deleted_dt");
        builder.Property(e => e.Address)
            .HasColumnName("address")
            .HasConversion<EmailAddressToStringConverter>();
        builder.UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasMany<EmailLabel>(Email.PrivateLabelsFieldName)
            .WithOne()
            .HasForeignKey(EmailLabelToEmailFkFieldName)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Confirmation)
            .WithOne()
            .HasForeignKey<Confirmation>(c => c.EmailId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(e => e.Labels);

        
    }
}