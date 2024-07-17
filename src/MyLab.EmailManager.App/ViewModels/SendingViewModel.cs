namespace MyLab.EmailManager.App.ViewModels;

public record SendingViewModel
(
    Guid Id,
    IReadOnlyDictionary<string, string> Selection,
    string SimpleContent,
    string TemplateId,
    IReadOnlyDictionary<string, string> TemplateArgs,
    IReadOnlyCollection<MessageViewModel> Messages
);