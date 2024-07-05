using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.Emails;

public class EmailViewModelDto
{
    public Guid Id { get; set; }
    public string? Address { get; set; }
    public Dictionary<string, string?>? Labels { get; set; }
    public List<MessageViewModel>? Tail { get; set; }
}