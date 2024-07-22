using MediatR;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.Features.CreateOrUpdateEmail;

public class CreateOrUpdateEmailHandler
    (
        IEmailRepository emailRepository,
        EmailCreationLogic creationLogic
    ) 
    : IRequestHandler<CreateOrUpdateEmailCommand>
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

            await emailRepository.SaveAsync(cancellationToken);
        }
        else
        {
            await creationLogic.CreateAsync
            (
                command.EmailId,
                command.Address,
                command.Labels,
                cancellationToken
            );
        }
    }
}