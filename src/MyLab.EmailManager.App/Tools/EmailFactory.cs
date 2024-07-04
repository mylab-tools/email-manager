using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.Tools
{
    static class EmailFactory
    {
        public static Email Create
        (
            Guid emailId,
            string address,
            IReadOnlyDictionary<string, string>? labelsKv
        )
        {
            var newEmail = new Email(emailId, new EmailAddress(address));
            if (labelsKv != null)
            {
                var labels = labelsKv
                    .Select(kv => new EmailLabel(kv.Key, kv.Value));
                newEmail.UpdateLabels(labels);
            }

            return newEmail;
        }
    }
}
