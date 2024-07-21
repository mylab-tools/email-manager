using System;
using System.Net;
using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.EmailManager.Client.Emails
{
    [Api("emails", Key = "email-manager")]
    public interface IEmailManagerEmailsV1
    {
        [Post]
        Task<Guid> CreateAsync([JsonContent]EmailDefDto emailDef);
        [Put]
        Task CreateOrUpdateAsync([Query("email_id")] Guid emailId, [JsonContent]EmailDefDto emailDef);
        [Get]
        [ExpectedCode(HttpStatusCode.NotFound)]
        Task<EmailViewModelDto?> GetAsync([Query("email_id")]Guid emailId);
        [Delete]
        Task DeleteAsync([Query("email_id")] Guid emailId);
    }
}
