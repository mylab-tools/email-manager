using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.UnitTests
{
    internal static class TestTools
    {
        public static T? GetId<T>(EntityEntry entry)
        {
            var name = entry.Metadata.FindPrimaryKey()!.Properties.Single().Name;
            var curVal = entry.Property(name).CurrentValue;

            return curVal == null ? default : (T)curVal;
        }
    }
}
