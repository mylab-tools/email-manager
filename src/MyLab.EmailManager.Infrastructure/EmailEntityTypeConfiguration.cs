using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Infrastructure;

class EmailEntityTypeConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("email");
        builder.Property<Guid>("id").IsRequired();
        builder.HasKey("id");
        builder.Property(e => e.Deleted).HasColumnName("deleted");
        builder.Property(e => e.DeleteDt).HasColumnName("deleted_dt");
        builder.Property(e => e.Address).HasColumnName("address");
        builder.UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasMany<EmailLabel>(nameof(Email.PrivateLabelsFieldName))
            .WithOne()
            .HasForeignKey("email_id")
            .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(e => e.Labels);
    }
}