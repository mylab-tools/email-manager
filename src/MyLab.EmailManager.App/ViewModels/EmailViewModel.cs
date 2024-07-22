namespace MyLab.EmailManager.App.ViewModels;

public record EmailViewModel
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public IReadOnlyDictionary<string, string?> Labels { get; set; }
    public IReadOnlyCollection<MessageViewModel> Tail { get; set; }
};