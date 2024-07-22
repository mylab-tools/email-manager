using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.ViewModels
{
    public class ConfirmationState
    {
        public bool Confirmed { get; set; }
        public ConfirmationStep Step { get; set; }
    }
}
