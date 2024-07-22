using MediatR;

namespace MyLab.EmailManager.App.Features.CompleteConfirmation
{
    public record CompleteConfirmationCommand(Guid Seed) : IRequest;
}
