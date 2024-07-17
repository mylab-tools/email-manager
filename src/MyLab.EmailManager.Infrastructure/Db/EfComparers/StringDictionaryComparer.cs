using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MyLab.EmailManager.Infrastructure.Db.EfComparers;

class StringDictionaryComparer : ValueComparer<IReadOnlyDictionary<string, string>>
{
    public StringDictionaryComparer() :
        base
        (
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => new Dictionary<string, string>(c)
        )
    {
    }
}