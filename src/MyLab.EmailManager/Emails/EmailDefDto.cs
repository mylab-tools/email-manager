using System.Collections.Generic;

#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Emails
#else
namespace MyLab.EmailManager.Emails
#endif
{
    public class EmailDefDto
    {
        public string? Address { get; set; }
        public Dictionary<string, string>? Labels { get; set; }
    }
}
