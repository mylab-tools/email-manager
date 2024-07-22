using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.ViewModels;

public record EmailMessageViewModel
(
    Guid Id,
    Guid EmailId,
    string Title,
    TextContent Content
);