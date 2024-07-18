using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Infrastructure.Db.EfConverters;

namespace MyLab.EmailManager.Infrastructure.Db.EfTypeConfigurations;

class EmailMessageEntityTypeConfiguration : IEntityTypeConfiguration<EmailMessage>
{
    public void Configure(EntityTypeBuilder<EmailMessage> builder)
    {
        builder.ToTable("message")
            .HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .IsRequired()
            .HasColumnName("id");
        builder.Property(m => m.EmailId)
            .HasColumnName("email_id");
        builder.HasOne<Email>()
            .WithMany()
            .HasForeignKey(m => m.EmailId)
            .IsRequired();
        builder.OwnsOne(m => m.Content)
            .Property(c => c.Text)
            .HasColumnName("content");
        builder.OwnsOne(m => m.Content)
            .Property(c => c.IsHtml)
            .HasColumnName("is_html");
        builder.Property(l => l.Title)
            .HasConversion<FilledStringToStringConverter>()
            .HasColumnName("title");
        builder.HasOne<Sending>()
            .WithMany(s => s.Messages)
            .IsRequired();
        builder.Property(m => m.CreateDt)
            .IsRequired()
            .HasColumnName("create_dt");
        builder.Property(m => m.SendDt)
            .HasColumnName("send_dt");
        builder.Property(m => m.EmailAddress)
            .HasColumnName("email_address")
            .HasConversion<EmailAddressToStringConverter>();
    }
}