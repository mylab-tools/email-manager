using System.Text.Json.Serialization;

#if CLIENTPROJ
using System;
using System.Collections.Generic;
using MyLab.EmailManager.Client.Common;

namespace MyLab.EmailManager.Client.Sendings
#else
using MyLab.EmailManager.Common;

namespace MyLab.EmailManager.Sendings
#endif
{
    public class SendingViewModelDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }
        [JsonPropertyName("selection")]
        public IReadOnlyDictionary<string, string> Selection { get; init; }
        [JsonPropertyName("simpleContent")]
        public string? SimpleContent { get; init; }
        [JsonPropertyName("templateId")]
        public string? TemplateId { get; init; }
        [JsonPropertyName("templateArgs")]
        public IReadOnlyDictionary<string, string>? TemplateArgs { get; init; }
        [JsonPropertyName("messages")]
        public IReadOnlyCollection<MessageViewModelDto> Messages { get; init; }
        [JsonPropertyName("sendingStatus")]
        public SendingStatusDto SendingStatus { get; init; }
        [JsonPropertyName("sendingStatusDt")]
        public DateTime SendingStatusDt { get; init; }
    }
}