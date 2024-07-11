using MediatR;
using MyLab.EmailManager.App.Tools;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.Features.CreateOrUpdateEmail;

public class CreateOrUpdateEmailHandler(IEmailRepository emailRepository) : IRequestHandler<CreateOrUpdateEmailCommand>
{
    public async Task Handle(CreateOrUpdateEmailCommand command, CancellationToken cancellationToken)
    {
        var email = await emailRepository.GetAsync(command.EmailId, cancellationToken);

        if (email != null)
        {
            email.Address = command.Address;
            email.UpdateLabels
            (
                command.Labels != null
                    ? command.Labels.Select(kv => new EmailLabel(kv.Key, kv.Value))
                    : Array.Empty<EmailLabel>()
            );
        }
        else
        {
            email = EmailFactory.Create
            (
                command.EmailId,
                command.Address,
                command.Labels
            );

            email.Confirmation = Confirmation.CreateNew(email.Id);

            await emailRepository.AddAsync(email, cancellationToken);
        }

        await emailRepository.SaveAsync(cancellationToken);
    }
}