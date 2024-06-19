using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure;

class EmailLabelEntityTypeConfiguration : IEntityTypeConfiguration<EmailLabel>
{
    public void Configure(EntityTypeBuilder<EmailLabel> builder)
    {
        builder.ToTable("labels");
        builder.Property<int>("id").ValueGeneratedOnAdd();
        builder.HasKey("id");
        builder.Property(l => l.Name).HasColumnName("name");
        builder.Property(l => l.Value).HasColumnName("value");
        builder.HasOne<Email>("email_id")
            .WithMany();
    }
}