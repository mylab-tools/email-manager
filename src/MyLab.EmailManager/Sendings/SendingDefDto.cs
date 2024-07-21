using System.Text.Json.Serialization;

#if CLIENTPROJ
using System.Collections.Generic;
namespace MyLab.EmailManager.Client.Sendings
#else
namespace MyLab.EmailManager.Sendings
#endif
{
    public class SendingDefDto
    {
        [JsonPropertyName("selection")]
        public IReadOnlyDictionary<string, string> Selection { get; init; }
        [JsonPropertyName("title")]
        public string Title { get; init; }
        [JsonPropertyName("simpleContent")]
        public string? SimpleContent { get; init; }
        [JsonPropertyName("templateId")]
        public string? TemplateId { get; init; }
        [JsonPropertyName("templateArgs")]
        public IReadOnlyDictionary<string, string>? Args { get; init; }
    }
}
