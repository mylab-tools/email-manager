using MyLab.EmailManager.App.Common.ViewModels;

namespace MyLab.EmailManager.App.Features.GetEmail;

public class EmailViewModel
{
    public required Guid Id { get; init; }
    public required string Address { get; init; }
    public required IReadOnlyDictionary<string, string?> Labels { get; init; }
    public required IReadOnlyCollection<MessageViewModel> Tail { get; init; }
}