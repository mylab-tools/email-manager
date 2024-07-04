using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.EfModels;

[Table("confirmation")]
public partial class DbConfirmation
{
    [Key]
    [Column("email_id")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid EmailId { get; set; }

    [Column("step")]
    public int Step { get; set; }

    [Column("step_dt")]
    [MaxLength(6)]
    public DateTime? StepDt { get; set; }

    [ForeignKey("EmailId")]
    [InverseProperty("Confirmation")]
    public virtual DbEmail Email { get; set; } = null!;
}
