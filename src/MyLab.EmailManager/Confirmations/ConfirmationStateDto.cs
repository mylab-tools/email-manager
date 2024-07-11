#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Confirmations
#else
namespace MyLab.EmailManager.Confirmations
#endif
{
    public class ConfirmationStateDto
    {
        public bool Confirmed { get; set; }
        public ConfirmationStateStep Step { get; set; }
    }
}
