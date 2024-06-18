using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities;

public class Sending
{
    public const string PrivateSelectionFieldName = nameof(_selection);

    List<EmailLabel> _selection = new();

    public IList<EmailLabel> Selection => _selection;

    public FilledString Title { get; }
    public SimpleMessageDef? SimpleMsg { get; }
    public GenericMessageDef? GenericMsg { get; }

    public Sending(FilledString title, SimpleMessageDef simpleMsg)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        SimpleMsg = simpleMsg ?? throw new ArgumentNullException(nameof(simpleMsg));
    }

    public Sending(FilledString title, GenericMessageDef genericMsg)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        GenericMsg = genericMsg ?? throw new ArgumentNullException(nameof(genericMsg));
    }
}