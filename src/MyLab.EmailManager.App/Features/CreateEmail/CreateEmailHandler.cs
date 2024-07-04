using AutoMapper;
using MediatR;
using MyLab.EmailManager.App.Tools;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Domain.Repositories;

namespace MyLab.EmailManager.App.Features.CreateEmail;

public class CreateEmailHandler(IEmailRepository emailRepository) : IRequestHandler<CreateEmailCommand, CreateEmailResponse>
{
    public async Task<CreateEmailResponse> Handle(CreateEmailCommand command, CancellationToken cancellationToken)
    {
        var newEmail = EmailFactory.Create
            (
                Guid.NewGuid(),
                command.Address,
                command.Labels
            );
        
        emailRepository.Add(newEmail);
        await emailRepository.SaveAsync(cancellationToken);

        return new CreateEmailResponse(newEmail.Id);
    }
}