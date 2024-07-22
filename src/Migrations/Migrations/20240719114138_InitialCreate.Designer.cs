﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Migrations;

#nullable disable

namespace Migrations.Migrations
{
    [DbContext(typeof(MigrationDbContext))]
    [Migration("20240719114138_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Confirmation", b =>
                {
                    b.Property<Guid>("EmailId")
                        .HasColumnType("char(36)")
                        .HasColumnName("email_id");

                    b.Property<Guid>("Seed")
                        .HasColumnType("char(36)")
                        .HasColumnName("seed");

                    b.HasKey("EmailId");

                    b.HasIndex("Seed");

                    b.ToTable("confirmation", (string)null);
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Email", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("address");

                    b.HasKey("Id");

                    b.ToTable("email", (string)null);
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.EmailMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreateDt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_dt");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email_address");

                    b.Property<Guid>("EmailId")
                        .HasColumnType("char(36)")
                        .HasColumnName("email_id");

                    b.Property<DateTime?>("SendDt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("send_dt");

                    b.Property<Guid>("SendingId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("EmailId");

                    b.HasIndex("SendingId");

                    b.ToTable("message", (string)null);
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Sending", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Selection")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("selection");

                    b.Property<string>("SimpleContent")
                        .HasColumnType("longtext")
                        .HasColumnName("simple_content");

                    b.Property<string>("TemplateArgs")
                        .HasColumnType("longtext")
                        .HasColumnName("template_args");

                    b.Property<string>("TemplateId")
                        .HasColumnType("longtext")
                        .HasColumnName("template_id");

                    b.HasKey("Id");

                    b.ToTable("sending", (string)null);
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.ValueObjects.EmailLabel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("value");

                    b.Property<Guid>("email_id")
                        .HasColumnType("char(36)");

                    b.HasKey("id");

                    b.HasIndex("email_id");

                    b.ToTable("label", (string)null);
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Confirmation", b =>
                {
                    b.HasOne("MyLab.EmailManager.Domain.Entities.Email", null)
                        .WithOne("Confirmation")
                        .HasForeignKey("MyLab.EmailManager.Domain.Entities.Confirmation", "EmailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("MyLab.EmailManager.Domain.ValueObjects.DatedValue<MyLab.EmailManager.Domain.ValueObjects.ConfirmationStep>", "Step", b1 =>
                        {
                            b1.Property<Guid>("ConfirmationEmailId")
                                .HasColumnType("char(36)");

                            b1.Property<DateTime?>("DateTime")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("step_dt");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("step");

                            b1.HasKey("ConfirmationEmailId");

                            b1.ToTable("confirmation");

                            b1.WithOwner()
                                .HasForeignKey("ConfirmationEmailId");
                        });

                    b.Navigation("Step")
                        .IsRequired();
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Email", b =>
                {
                    b.OwnsOne("MyLab.EmailManager.Domain.ValueObjects.DatedValue<bool>", "Deletion", b1 =>
                        {
                            b1.Property<Guid>("EmailId")
                                .HasColumnType("char(36)");

                            b1.Property<DateTime?>("DateTime")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("deleted_dt");

                            b1.Property<bool>("Value")
                                .HasColumnType("tinyint(1)")
                                .HasColumnName("deleted");

                            b1.HasKey("EmailId");

                            b1.ToTable("email");

                            b1.WithOwner()
                                .HasForeignKey("EmailId");
                        });

                    b.Navigation("Deletion")
                        .IsRequired();
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.EmailMessage", b =>
                {
                    b.HasOne("MyLab.EmailManager.Domain.Entities.Email", null)
                        .WithMany()
                        .HasForeignKey("EmailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyLab.EmailManager.Domain.Entities.Sending", null)
                        .WithMany("Messages")
                        .HasForeignKey("SendingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("MyLab.EmailManager.Domain.ValueObjects.DatedValue<MyLab.EmailManager.Domain.ValueObjects.SendingStatus>", "SendingStatus", b1 =>
                        {
                            b1.Property<Guid>("EmailMessageId")
                                .HasColumnType("char(36)");

                            b1.Property<DateTime?>("DateTime")
                                .IsRequired()
                                .HasColumnType("datetime(6)")
                                .HasColumnName("sending_status_dt");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("sending_status");

                            b1.HasKey("EmailMessageId");

                            b1.ToTable("message");

                            b1.WithOwner()
                                .HasForeignKey("EmailMessageId");
                        });

                    b.OwnsOne("MyLab.EmailManager.Domain.ValueObjects.TextContent", "Content", b1 =>
                        {
                            b1.Property<Guid>("EmailMessageId")
                                .HasColumnType("char(36)");

                            b1.Property<bool>("IsHtml")
                                .HasColumnType("tinyint(1)")
                                .HasColumnName("is_html");

                            b1.Property<string>("Text")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("content");

                            b1.HasKey("EmailMessageId");

                            b1.ToTable("message");

                            b1.WithOwner()
                                .HasForeignKey("EmailMessageId");
                        });

                    b.Navigation("Content")
                        .IsRequired();

                    b.Navigation("SendingStatus")
                        .IsRequired();
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Sending", b =>
                {
                    b.OwnsOne("MyLab.EmailManager.Domain.Entities.Sending.SendingStatus#DatedValue", "SendingStatus", b1 =>
                        {
                            b1.Property<Guid>("SendingId")
                                .HasColumnType("char(36)");

                            b1.Property<DateTime?>("DateTime")
                                .IsRequired()
                                .HasColumnType("datetime(6)")
                                .HasColumnName("sending_status_dt");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("sending_status");

                            b1.HasKey("SendingId");

                            b1.ToTable("sending");

                            b1.WithOwner()
                                .HasForeignKey("SendingId");
                        });

                    b.Navigation("SendingStatus")
                        .IsRequired();
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.ValueObjects.EmailLabel", b =>
                {
                    b.HasOne("MyLab.EmailManager.Domain.Entities.Email", null)
                        .WithMany("_labels")
                        .HasForeignKey("email_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Email", b =>
                {
                    b.Navigation("Confirmation");

                    b.Navigation("_labels");
                });

            modelBuilder.Entity("MyLab.EmailManager.Domain.Entities.Sending", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
