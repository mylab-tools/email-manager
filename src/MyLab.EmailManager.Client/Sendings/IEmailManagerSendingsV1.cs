using System;
using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.EmailManager.Client.Sendings
{
    [Api("sendings", Key = "email-manager")]
    public interface IEmailManagerSendingsV1
    {
        [Post]
        Task<Guid> CreateAsync([JsonContent] SendingDefDto sendingDef);

        [Get("{sending_id}")]
        Task<SendingViewModelDto> GetAsync([Path("sending_id")] Guid sendingId);
    }
}
