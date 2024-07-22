using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MyLab.EmailManager.Domain.Exceptions;

namespace MyLab.EmailManager.Domain.ValueObjects;

public class EmailAddress
{
    public static readonly Regex Regex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

    public static void Validate(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainValidationException("Address is empty");
        if (!Regex.IsMatch(address))
            throw new DomainValidationException("Invalid address value");
    }

    public string Value { get; }

    public EmailAddress(string value)
    {
        Validate(value);
        Value = value;
    }

    public static implicit operator EmailAddress(string stringValue)
    {
        return new EmailAddress(stringValue);
    }

    public override string ToString()
    {
        return Value;
    }
}