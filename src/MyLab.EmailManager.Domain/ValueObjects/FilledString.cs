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

        public string Value { get; }
        public FilledString(string value)
        {
            Validate(value);
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator FilledString(string value)
        {
            return new FilledString(value);
        }
    }
}
