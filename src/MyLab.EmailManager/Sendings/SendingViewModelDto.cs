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
        public Guid Id { get; init; } 
        public IReadOnlyDictionary<string, string> Selection { get; init; }
        public string? SimpleContent { get; init; }
        public string? TemplateId { get; init; }
        public IReadOnlyDictionary<string, string>? TemplateArgs { get; init; }
        public IReadOnlyCollection<MessageViewModelDto> Messages { get; init; }
        public SendingStatusDto SendingStatus { get; init; }
        public DateTime SendingStatusDt { get; init; }
    }
}