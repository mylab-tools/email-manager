using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities
{
    public class EmailLabel(FilledString name, string? value)
    {
        public FilledString Name { get; } = name;
        public string? Value { get; } = value;
    }
}
