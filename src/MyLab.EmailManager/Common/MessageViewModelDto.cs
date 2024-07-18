using System;

#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Common
#else
namespace MyLab.EmailManager.Common
#endif
{
    public class MessageViewModelDto
    {
        public Guid EmailId { get; set; }
        public string? EmailAddress{ get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool IsHtml { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime? SendDt { get; set; }

        public SendingStatusDto SendingStatus { get; set; }
    }
}