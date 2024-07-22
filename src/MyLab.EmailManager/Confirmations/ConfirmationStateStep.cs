#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Confirmations
#else
namespace MyLab.EmailManager.Confirmations
#endif
{
    public enum ConfirmationStateStep
    {
        Undefined,
        Created,
        Sent,
        Confirmed
    }
}