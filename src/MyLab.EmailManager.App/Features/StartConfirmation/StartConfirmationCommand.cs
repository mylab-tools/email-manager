using MediatR;

namespace MyLab.EmailManager.App.Features.StartConfirmation
{
    public record StartConfirmationCommand(Guid EmailId) : IRequest;
}
