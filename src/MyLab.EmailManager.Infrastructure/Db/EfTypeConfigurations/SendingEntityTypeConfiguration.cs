using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db.EfConverters;
using MyLab.EmailManager.Infrastructure.Db.EfComparers;

namespace MyLab.EmailManager.Infrastructure.Db.EfTypeConfigurations;

public class SendingEntityTypeConfiguration : IEntityTypeConfiguration<Sending>
{
    public void Configure(EntityTypeBuilder<Sending> builder)
    {
        builder.ToTable("sending")
            .HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .IsRequired();
        builder.Property(d => d.SimpleContent)
            .HasConversion<SimpleContentToStringConverter>()
            .HasColumnName("simple_content");
        builder.Property(d => d.GenericContent)
            .HasConversion<ObjectToJsonStringConverter<GenericMessageContent>>()
            .HasColumnName("generic_content");
        builder.Property(d => d.Selection)
            .HasConversion<ObjectToJsonStringConverter<EmailLabel[]>>(new EmailLabelArrayComparer())
            .HasColumnName("selection");
        builder.Property(d => d.Title)
            .HasConversion<FilledStringToStringConverter>()
            .HasColumnName("title");
    }
}