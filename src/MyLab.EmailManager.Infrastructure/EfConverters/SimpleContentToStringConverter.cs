using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.EfConverters
{
    class SimpleContentToStringConverter() : ValueConverter<SimpleMessageContent, string>(ContentToString, StringToContent)
    {
        static Expression<Func<SimpleMessageContent, string>> ContentToString =>
            email => email.Text;

        static Expression<Func<string, SimpleMessageContent>> StringToContent =>
            str => new SimpleMessageContent(str);
    }
}
