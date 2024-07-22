#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Common
#else
namespace MyLab.EmailManager.Common
#endif
{
    public enum SendingStatusDto
    {
        Undefined,
        Pending,
        Sending,
        Sent
    }
}
