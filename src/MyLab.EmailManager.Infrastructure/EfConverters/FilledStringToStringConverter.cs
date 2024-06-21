using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.EfConverters;

class FilledStringToStringConverter() : ValueConverter<FilledString, string>(FilledToString, StringToFilled)
{
    static Expression<Func<FilledString, string>> FilledToString =>
        str => str.Text;

    static Expression<Func<string, FilledString>> StringToFilled =>
        str => new FilledString(str);
}