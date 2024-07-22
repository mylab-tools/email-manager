using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#if CLIENTPROJ
using MyLab.EmailManager.Client.Common;
namespace MyLab.EmailManager.Client.Emails
#else
using MyLab.EmailManager.Common;
namespace MyLab.EmailManager.Emails
#endif
{
    public class EmailViewModelDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        [JsonPropertyName("labels")]
        public Dictionary<string, string?>? Labels { get; set; }
        [JsonPropertyName("messagesTail")]
        public List<MessageViewModelDto>? MessagesTail { get; set; }
    }
}