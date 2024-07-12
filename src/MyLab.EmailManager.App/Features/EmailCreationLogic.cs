using MyLab.EmailManager.App.ConfirmationStuff;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.Features
{
    public class EmailCreationLogic(IEmailRepository emailRepository, ConfirmationMessageSender messageSender)
    {
        public async Task CreateAsync
            (
                Guid newEmailId, 
                string address,
                IReadOnlyDictionary<string,string>? labels,
                CancellationToken cancellationToken)
        {
            var newEmail = new Email(newEmailId, new EmailAddress(address));
            newEmail.Confirmation = Confirmation.CreateNew(newEmail.Id);

            if (labels != null)
            {
                newEmail.UpdateLabels(labels.Select(kv => new EmailLabel(kv.Key, kv.Value)));
            }

            await emailRepository.AddAsync(newEmail, cancellationToken);
            await emailRepository.SaveAsync(cancellationToken);

            await messageSender.SendAsync(address, labels);
        }
    }
}
