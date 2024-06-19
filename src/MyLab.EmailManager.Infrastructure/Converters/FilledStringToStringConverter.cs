using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.Converters;

class FilledStringToStringConverter : ValueConverter<FilledString, string>
{
    static Expression<Func<FilledString, string>> FilledToString =>
        str => str.Text;

    static Expression<Func<string, FilledString>> StringToFilled =>
        str => new FilledString(str);

    public FilledStringToStringConverter()
        : base(FilledToString, StringToFilled)
    {
    }
}