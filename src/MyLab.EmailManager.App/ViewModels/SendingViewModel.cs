using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.ViewModels;

public class SendingViewModel
{
    public required Guid Id { get; init; }
    public required IReadOnlyDictionary<string, string> Selection { get; init; }
    public string? SimpleContent { get; init; }
    public string? TemplateId { get; init; }
    public IReadOnlyDictionary<string, string>? TemplateArgs { get; init; }
    public required IReadOnlyCollection<MessageViewModel> Messages { get; init; }
    public SendingStatus SendingStatus { get; init; }
};