using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace MyLab.EmailManager.App.Features.GetSending
{
    public class GetSendingHandler(ReadDbContext db, IMapper mapper) : IRequestHandler<GetSendingQuery, SendingViewModel?>
    {
        public async Task<SendingViewModel?> Handle(GetSendingQuery request, CancellationToken cancellationToken)
        {
            var dbSending = await db.Sendings
                .Where(s => s.Id == request.SendingId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            try
            {
                return mapper.Map<SendingViewModel>(dbSending);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
