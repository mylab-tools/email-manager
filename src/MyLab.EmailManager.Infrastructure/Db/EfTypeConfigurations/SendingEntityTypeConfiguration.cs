using System.Collections;
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
        builder.Property(s => s.Selection)
            .HasConversion<ObjectToJsonStringConverter<EmailLabel[]>>(new EmailLabelArrayComparer())
            .HasColumnName("selection");
        builder.HasMany(s => s.Messages)
            .WithOne();
        builder.Property(s => s.SimpleContent)
            .HasConversion<FilledStringToStringConverter>()
            .HasColumnName("simple_content");
        builder.Property(s => s.TemplateId)
            .HasConversion<FilledStringToStringConverter>()
            .HasColumnName("template_id");
        builder.Property(s => s.TemplateArgs)
            .HasConversion<ObjectToJsonStringConverter<IReadOnlyDictionary<string,string>>>
                (
                    new StringDictionaryComparer()
                )
            .HasColumnName("template_args");
    }
}