using System;
using System.Text.Json.Serialization;

#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Common
#else
namespace MyLab.EmailManager.Common
#endif
{
    public class MessageViewModelDto
    {
        [JsonPropertyName("emailId")]
        public Guid EmailId { get; set; }
        [JsonPropertyName("emailAddress")]
        public string? EmailAddress{ get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        [JsonPropertyName("isHtml")]
        public bool IsHtml { get; set; }
        [JsonPropertyName("createDt")]
        public DateTime CreateDt { get; set; }
        [JsonPropertyName("sendDt")]
        public DateTime? SendDt { get; set; }
        [JsonPropertyName("sendingStatus")]
        public SendingStatusDto SendingStatus { get; set; }
        [JsonPropertyName("sendingStatusDt")]
        public DateTime SendingStatusDt { get; set; }
    }
}