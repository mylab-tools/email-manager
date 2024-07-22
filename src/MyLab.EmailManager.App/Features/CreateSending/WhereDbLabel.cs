using System.Linq.Expressions;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace MyLab.EmailManager.App.Features.CreateSending;

static class WhereDbLabel
{
    public static Expression<Func<DbLabel, bool>> EqualsTo(KeyValuePair<string, string> kv)
    {
        return db => db.Name == kv.Key && db.Value == kv.Value;
    }

    public static Expression<Func<DbLabel, bool>> In(IReadOnlyDictionary<string, string> selection)
    {
        if (selection is not {Count: >0})
            return l => true;

        Expression? eBody = null;
        var inputParameter = Expression.Parameter(typeof(DbLabel));

        foreach (var kv in selection)
        {
            if (eBody == null)
            {
                eBody = CreateKvPairToLabelExpression(inputParameter, kv);
            }
            else
            {
                eBody = Expression.Or
                (
                    eBody,
                    CreateKvPairToLabelExpression(inputParameter, kv)
                );
            }
        }

        return Expression.Lambda<Func<DbLabel, bool>>
                (
                    eBody!,
                    inputParameter
                );
    }

    private static BinaryExpression CreateKvPairToLabelExpression(ParameterExpression p, KeyValuePair<string, string> kv)
    {
        return Expression.AndAlso
        (
            Expression.Equal
            (
                Expression.Property
                (
                    p,
                    typeof(DbLabel).GetProperty(nameof(DbLabel.Name))!
                ),
                Expression.Constant(kv.Key)
            ),
            Expression.Equal
            (
                Expression.Property
                (
                    p,
                    typeof(DbLabel).GetProperty(nameof(DbLabel.Value))!
                ),
                Expression.Constant(kv.Value)
            )
        );
    }
}