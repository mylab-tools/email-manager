using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

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
            newEmail.Confirmation = Confirmation.CreateNew(newEmail.Id);

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
