using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.ViewModels
{
    public record MessageViewModel
        (
            Guid Id,
            Guid EmailId,
            string EmailAddress,
            DateTime CreateDt,
            DateTime? SendDt,
            string Title,
            string Content,
            bool IsHtml, 
            SendingStatus SendingStatus
        );
}
