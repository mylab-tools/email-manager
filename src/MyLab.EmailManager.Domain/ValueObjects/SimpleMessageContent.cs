namespace MyLab.EmailManager.Domain.ValueObjects
{
    public class SimpleMessageContent(string text) : FilledString(text);
}
