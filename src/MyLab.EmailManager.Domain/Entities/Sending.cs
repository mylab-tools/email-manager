using MyLab.EmailManager.Domain.Dto;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities;

public class Sending
{
    public Guid Id { get; private set; }
    public EmailLabel[] Selection { get; }
    public FilledString Title { get; }
    public SimpleMessageContent? SimpleContent { get; }
    public GenericMessageContent? GenericContent { get; }

    private Sending(){}

    public Sending(Guid id, SendMessageDef message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        Id = id;
        
        if (message.Selection.Length == 0)
            throw new DomainException("Selection labels are required");
        if (message is { GenericContent: null, SimpleContent: null })
            throw new DomainException("Any message content is required");
        if (message is { GenericContent: not null, SimpleContent: not null })
            throw new DomainException("Only one message content case is supported");

        Title = message.Title;
        Selection = message.Selection;
        SimpleContent = message.SimpleContent;
        GenericContent  = message.GenericContent;
    }
}