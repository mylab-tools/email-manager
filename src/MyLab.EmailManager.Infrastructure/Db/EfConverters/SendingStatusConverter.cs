using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.Db.EfConverters
{
    public class SendingStatusConverter() : ValueConverter<SendingStatus, string>(StatusToString, StatusToStep)
    {
        static Expression<Func<SendingStatus, string>> StatusToString =>
            step => step.ToLiteral();

        private static Expression<Func<string, SendingStatus>> StatusToStep =>
            str => Enum.Parse<SendingStatus>(str);
    }
}
