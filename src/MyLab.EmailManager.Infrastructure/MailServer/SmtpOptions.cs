using System.ComponentModel.DataAnnotations;

namespace MyLab.EmailManager.Infrastructure.MailServer
{
    public class SmtpOptions
    {
        [Required]
        public string? Host { get; set; }

        public ushort Port { get; set; } = 587;
        [Required]
        public string? Login { get; set; }
        [Required]
        public string? Password{ get; set; }
        
        public string? SenderName { get; set; }
        [Required]
        public string? SenderEmail { get; set; }
    }
}
