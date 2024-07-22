using MediatR;
using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.App.Features.GetSending
{
    public record GetSendingQuery(Guid SendingId) : IRequest<SendingViewModel?>;
}
