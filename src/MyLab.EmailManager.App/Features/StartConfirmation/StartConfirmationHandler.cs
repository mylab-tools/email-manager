using MediatR;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.App.Features.StartConfirmation
{
    public class StartConfirmationHandler(IConfirmationRepository repo) : IRequestHandler<StartConfirmationCommand>
    {
        public async Task Handle(StartConfirmationCommand request, CancellationToken cancellationToken)
        {
            var confirmation = await repo.GetAsync(request.EmailId, cancellationToken);
            if (confirmation != null)
            {
                confirmation.Reset();
            }
            else
            {
                confirmation = Confirmation.CreateNew(request.EmailId);
                await repo.AddAsync(confirmation, cancellationToken);
            }

            await repo.SaveAsync(cancellationToken);
        }
    }
}
