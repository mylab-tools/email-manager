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
    public record SendingViewModelDto(
        Guid Id,
        IReadOnlyDictionary<string, string> Selection,
        string? SimpleContent,
        string? TemplateId,
        IReadOnlyDictionary<string, string>? TemplateArgs,
        IReadOnlyCollection<MessageViewModelDto> Messages
    );
}