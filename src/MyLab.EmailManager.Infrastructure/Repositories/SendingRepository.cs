using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db;

namespace MyLab.EmailManager.Infrastructure.Repositories
{
    public class SendingRepository(DomainDbContext dbContext) : ISendingRepository
    {
        public void Add(Sending sending)
        {
            dbContext.Sendings.Add(sending);
        }

        public async Task<IList<Sending>> GetAsync(Expression<Func<Sending, bool>> specification, CancellationToken cancellationToken)
        {
            return await dbContext.Sendings
                .Where(specification)
                .Include(s => s.Messages)
                .ToListAsync(cancellationToken);
        }

        public Task SaveAsync(CancellationToken cancellationToken)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
