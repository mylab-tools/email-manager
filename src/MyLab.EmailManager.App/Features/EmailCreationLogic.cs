using MyLab.EmailManager.App.ConfirmationStuff;
using MyLab.EmailManager.App.Tools;
using MyLab.EmailManager.Domain.Repositories;

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
            var newEmail = EmailFactory.Create
            (
                newEmailId,
                address,
                labels
            );

            await emailRepository.AddAsync(newEmail, cancellationToken);
            await emailRepository.SaveAsync(cancellationToken);

            await messageSender.SendAsync(address, labels);
        }
    }
}
