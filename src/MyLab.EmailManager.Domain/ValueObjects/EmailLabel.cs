namespace MyLab.EmailManager.Domain.ValueObjects
{
    public class EmailLabel(FilledString name, string? value)
    {
        public FilledString Name { get; } = name;
        public string? Value { get; } = value;
    }
}
