using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.Db.EfConverters
{
    public class ConfirmationStepConverter() : ValueConverter<ConfirmationStep, string>(StepToString, StringToStep)
    {
        static Expression<Func<ConfirmationStep, string>> StepToString =>
            step => (Enum.GetName(step) ?? "undefined").ToLower();

        private static Expression<Func<string, ConfirmationStep>> StringToStep =>
            str => Enum.Parse<ConfirmationStep>(str);
    }
}
