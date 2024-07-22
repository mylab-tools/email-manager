using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; init; }
        public Guid EmailId { get; init; }
        public required string EmailAddress { get; init; }
        public DateTime CreateDt { get; init; }
        public DateTime? SendDt { get; init; }
        public required string Title { get; init; } 
        public required string Content { get; init; }
        public bool IsHtml { get; init; }
        public DatedValue<SendingStatus> SendingStatus { get; init; } = DatedValue<SendingStatus>.CreateUnset();
    }
}
