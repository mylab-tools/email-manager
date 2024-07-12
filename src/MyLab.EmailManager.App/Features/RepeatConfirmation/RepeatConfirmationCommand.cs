using MediatR;

namespace MyLab.EmailManager.App.Features.RepeatConfirmation
{
    public record RepeatConfirmationCommand(Guid EmailId) : IRequest;
}
