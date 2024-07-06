using System;
using System.Collections.Generic;

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
        public Guid Id { get; set; }
        public string? Address { get; set; }
        public Dictionary<string, string?>? Labels { get; set; }
        public List<MessageViewModelDto>? Tail { get; set; }
    }
}