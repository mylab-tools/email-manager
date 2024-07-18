using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure
{
    public static class SendingStatusExtensions
    {
        public static string ToLiteral(this SendingStatus status)
        {
            return Enum.GetName(status)?.ToLower() ?? "undefined";
        }
    }
}
