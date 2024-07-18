using MediatR;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.EmailManager.Infrastructure.MailServer;

namespace MyLab.EmailManager.App.Features.SendPendingMessages
{
    public class SendPendingMessagesHandler(ReadDbContext dbContext, IMailServerIntegration mailServerIntegration) : IRequestHandler<SendPendingMessagesCommand>
    {
        public async Task Handle(SendPendingMessagesCommand request, CancellationToken cancellationToken)
        {
            //var pendingMails = await dbContext.Messages
            //    .Where(m => !m.Email.Deleted && !m.SendDt.HasValue)
            //    .ToArrayAsync(cancellationToken);

            //foreach (var mail in pendingMails)
            //{
            //    dbContext.Sendings
            //        .Where(s => s.Id == mail.SendingId)
            //        .Include(s => s.);
            //}

            throw new NotImplementedException();
        }
    }
}
