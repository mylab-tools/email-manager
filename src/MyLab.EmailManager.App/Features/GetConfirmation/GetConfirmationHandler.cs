using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace MyLab.EmailManager.App.Features.GetConfirmation
{
    public class GetConfirmationHandler(ReadDbContext dbCtx, IMapper mapper) : IRequestHandler<GetConfirmationCommand, ConfirmationState?>
    {
        public async Task<ConfirmationState?> Handle(GetConfirmationCommand request, CancellationToken cancellationToken)
        {
            var dbConfirmation = await dbCtx.Confirmations.FirstOrDefaultAsync
            (
                c => c.EmailId == request.EmailId,
                cancellationToken
            );

            return dbConfirmation != null
                ? mapper.Map<ConfirmationState>(dbConfirmation)
                : null;
        }
    }
}
