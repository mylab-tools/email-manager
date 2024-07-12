using MediatR;
using Microsoft.Extensions.Options;
using MyLab.EmailManager.App.ConfirmationStuff;
using MyLab.EmailManager.App.Tools;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.App.Features.CreateEmail;

public class CreateEmailHandler(EmailCreationLogic creationLogic) 
    : IRequestHandler<CreateEmailCommand, CreateEmailResponse>
{
    public async Task<CreateEmailResponse> Handle(CreateEmailCommand command, CancellationToken cancellationToken)
    {
        var newEmailId = Guid.NewGuid();

        await creationLogic.CreateAsync
            (
                newEmailId, 
                command.Address, 
                command.Labels, 
                cancellationToken
            );

        return new CreateEmailResponse(newEmailId);
    }
}