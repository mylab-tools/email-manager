using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.EfComparers
{
    class EmailLabelArrayComparer : ValueComparer<EmailLabel[]>
    {
        public EmailLabelArrayComparer() : 
            base
            (
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToArray()
            )
        {
        }
    }
}
