using MediatR;
using MyLab.EmailManager.App.Exceptions;
using MyLab.EmailManager.Domain.Repositories;

namespace MyLab.EmailManager.App.Features.CompleteConfirmation;

public class CompleteConfirmationHandler(IConfirmationRepository repo) : IRequestHandler<CompleteConfirmationCommand>
{
    public async Task Handle(CompleteConfirmationCommand request, CancellationToken cancellationToken)
    {
        var confirmation = await repo.GetBySeedAsync(request.Seed, cancellationToken);

        if (confirmation == null)
            throw new NotFoundException("Confirmation not found");

        confirmation.Complete(request.Seed);

        await repo.SaveAsync(cancellationToken);
    }
}