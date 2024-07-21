using System.ComponentModel.DataAnnotations;

namespace MyLab.EmailManager.Infrastructure.MailServer
{
    public class SmtpOptions
    {
        [Required]
        public required string Host { get; set; }

        public ushort Port { get; set; } = 587;
        [Required]
        public required string Login { get; set; }
        [Required]
        public required string Password{ get; set; }
        
        public string? SenderName { get; set; }
        [Required]
        public required string SenderEmail { get; set; }
    }
}
