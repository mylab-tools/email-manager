﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.EfConverters;

namespace MyLab.EmailManager.Infrastructure;

class EmailLabelEntityTypeConfiguration : IEntityTypeConfiguration<EmailLabel>
{
    public void Configure(EntityTypeBuilder<EmailLabel> builder)
    {
        builder.ToTable("labels");
        builder.Property<int>("id").ValueGeneratedOnAdd();
        builder.HasKey("id");
        builder.Property(l => l.Name)
            .HasConversion<FilledStringToStringConverter>()
            .HasColumnName("name");
        builder.Property(l => l.Value).HasColumnName("value");
    }
}