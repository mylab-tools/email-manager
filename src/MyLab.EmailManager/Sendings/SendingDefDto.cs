#if CLIENTPROJ
using System.Collections.Generic;
namespace MyLab.EmailManager.Client.Sendings
#else
namespace MyLab.EmailManager.Sendings
#endif
{
    public record SendingDefDto(
        IReadOnlyDictionary<string, string> Selection,
        string Title,
        string? SimpleContent,
        string? TemplateId,
        IReadOnlyDictionary<string, string>? Args
    );
}
