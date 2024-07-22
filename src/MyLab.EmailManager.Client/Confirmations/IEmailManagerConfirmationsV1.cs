using System;
using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.EmailManager.Client.Confirmations
{
    [Api(Key = "email-manager")]
    public interface IEmailManagerConfirmationsV1
    {
        [Post("emails/{email_id}/confirmation/new")]
        Task RepeatAsync([Path("email_id")] Guid emailId);

        [Get("emails/{email_id}/confirmation/state")]
        Task<ConfirmationStateDto> GetStateAsync([Path("email_id")] Guid emailId);

        [Post("confirmations/completed/{seed}")]
        Task CompleteAsync([Path] Guid seed);
    }
}
