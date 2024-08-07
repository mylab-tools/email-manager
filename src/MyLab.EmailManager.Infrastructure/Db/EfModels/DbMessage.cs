using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.Db.EfModels;

[Table("message")]
[Index("SendingId", Name = "IX_message_SendingId")]
[Index("EmailId", Name = "IX_message_email_id")]
public partial class DbMessage
{
    [Key]
    [Column("id")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid Id { get; set; }

    [Column("email_id")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid EmailId { get; set; }

    [Column("create_dt")]
    [MaxLength(6)]
    public DateTime CreateDt { get; set; }

    [Column("send_dt")]
    [MaxLength(6)]
    public DateTime? SendDt { get; set; }

    [Column("email_address")]
    public string EmailAddress { get; set; } = null!;

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("content")]
    public string Content { get; set; } = null!;

    [Column("is_html")]
    public bool IsHtml { get; set; }

    [Column("sending_status")]
    public string SendingStatus { get; set; } = null!;

    [Column("sending_status_dt")]
    [MaxLength(6)]
    public DateTime SendingStatusDt { get; set; }

    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid SendingId { get; set; }

    [ForeignKey("EmailId")]
    [InverseProperty("Messages")]
    public virtual DbEmail Email { get; set; } = null!;

    [ForeignKey("SendingId")]
    [InverseProperty("Messages")]
    public virtual DbSending Sending { get; set; } = null!;
}
