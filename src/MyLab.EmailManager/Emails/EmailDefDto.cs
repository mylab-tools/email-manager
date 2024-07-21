using System.Collections.Generic;
using System.Text.Json.Serialization;

#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Emails
#else
namespace MyLab.EmailManager.Emails
#endif
{
    public class EmailDefDto
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        [JsonPropertyName("labels")]
        public Dictionary<string, string>? Labels { get; set; }
    }
}
