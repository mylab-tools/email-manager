using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities;

public record Sending
{
    public Guid Id { get; }
    public EmailLabel[] Selection { get; }
    public DatedValue<SendingStatus> SendingStatus { get; set; } = DatedValue<SendingStatus>.CreateUnset();
    public FilledString? SimpleContent { get; private set; }
    public FilledString? TemplateId { get; private set; }
    public IReadOnlyDictionary<string, string>? TemplateArgs { get; private set; }
    public IList<EmailMessage>? Messages { get; set; }

    public Sending(Guid id, EmailLabel[] selection, FilledString simpleContent)
    {
        Id = id;
        Selection = selection;
        SimpleContent = simpleContent;
    }

    public Sending
    (
        Guid id, 
        EmailLabel[] selection,
        FilledString templateId,
        IReadOnlyDictionary<string, string>? templateArgs
    )
    {
        Id = id;
        Selection = selection;
        TemplateId = templateId;
        TemplateArgs = templateArgs;
    }
};