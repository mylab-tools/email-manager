﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Infrastructure;

class ConfirmationEntityTypeConfiguration : IEntityTypeConfiguration<Confirmation>
{
    public void Configure(EntityTypeBuilder<Confirmation> builder)
    {
        builder.ToTable("confirmations")
            .HasKey(c => c.EmailId);
        builder.Property<int>("email_id")
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