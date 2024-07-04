using MediatR;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.Features.CreateEmail;

public class CreateEmailHandler(IEmailRepository emailRepository) : IRequestHandler<CreateEmailCommand, CreateEmailResponse>
{
    public async Task<CreateEmailResponse> Handle(CreateEmailCommand command, CancellationToken cancellationToken)
    {
        var newEmailId = Guid.NewGuid();
        var newEmail = new Email(newEmailId, new EmailAddress(command.Address));
        if (command.Labels != null)
        {
            var labels = command.Labels
                .Select(kv => new EmailLabel(kv.Key, kv.Value));
            newEmail.UpdateLabels(labels);
        }

        await emailRepository.AddAsync(newEmail);

        return new CreateEmailResponse(newEmailId);
    }
}