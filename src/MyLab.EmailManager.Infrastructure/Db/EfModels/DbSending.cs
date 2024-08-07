using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.Db.EfModels;

[Table("sending")]
public partial class DbSending
{
    [Key]
    [Column("id")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid Id { get; set; }

    [Column("selection")]
    public string Selection { get; set; } = null!;

    [Column("sending_status")]
    public string SendingStatus { get; set; } = null!;

    [Column("sending_status_dt")]
    [MaxLength(6)]
    public DateTime SendingStatusDt { get; set; }

    [Column("simple_content")]
    public string? SimpleContent { get; set; }

    [Column("template_id")]
    public string? TemplateId { get; set; }

    [Column("template_args")]
    public string? TemplateArgs { get; set; }

    [InverseProperty("Sending")]
    public virtual ICollection<DbMessage> Messages { get; set; } = new List<DbMessage>();
}
