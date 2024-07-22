using System.ComponentModel.DataAnnotations;
using MyLab.EmailManager.Domain.Exceptions;

namespace MyLab.EmailManager.Domain.ValueObjects
{
    public class FilledString
    {
        public static void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("String has no value");
        }

        public string Text { get; }
        public FilledString(string text)
        {
            Validate(text);
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator FilledString(string value)
        {
            return new FilledString(value);
        }
    }
}
