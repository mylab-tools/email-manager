using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyLab.EmailManager.Domain.ValueObjects;

public class EmailAddress
{
    public static readonly Regex Regex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

    public static void Validate(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ValidationException("Address is empty");
        if (!Regex.IsMatch(address))
            throw new ValidationException("Invalid address value");
    }

    public string Address { get; }

    public EmailAddress(string address)
    {
        Validate(address);
        Address = address;
    }

    public static implicit operator EmailAddress(string stringValue)
    {
        return new EmailAddress(stringValue);
    }
}