using MediatR;
using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.App.Features.GetConfirmation
{
    public record GetConfirmationCommand(Guid EmailId) : IRequest<ConfirmationState?>;
}
