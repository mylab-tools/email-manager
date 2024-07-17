using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Infrastructure.Db;

namespace MyLab.EmailManager.Infrastructure.Repositories
{
    public class SendingRepository(DomainDbContext dbContext) : ISendingRepository
    {
        public void Add(Sending sending)
        {
            dbContext.Sendings.Add(sending);
        }

        public Task SaveAsync(CancellationToken cancellationToken)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
