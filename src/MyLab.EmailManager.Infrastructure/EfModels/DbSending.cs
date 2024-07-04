using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.EfModels;

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

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("simple_content")]
    public string? SimpleContent { get; set; }

    [Column("generic_content")]
    public string? GenericContent { get; set; }
}
