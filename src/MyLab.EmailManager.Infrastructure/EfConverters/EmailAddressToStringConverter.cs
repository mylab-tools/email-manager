using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.EfConverters;

class EmailAddressToStringConverter() : ValueConverter<EmailAddress, string>(AddressToString, StringToAddress)
{
    static Expression<Func<EmailAddress, string>> AddressToString =>
        email => email.Value;

    static Expression<Func<string, EmailAddress>> StringToAddress =>
        str => new EmailAddress(str);
}