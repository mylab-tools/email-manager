using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Infrastructure.Db;

namespace MyLab.EmailManager.Infrastructure.Repositories
{
    public class ConfirmationRepository(DomainDbContext dbContext) : IConfirmationRepository
    {
        public async Task AddAsync(Confirmation confirmation, CancellationToken cancellationToken)
        {
            await dbContext.Confirmations.AddAsync(confirmation, cancellationToken);
        }

        public Task<Confirmation?> GetAsync(Guid emailId, CancellationToken cancellationToken)
        {
            return dbContext.Confirmations.FirstOrDefaultAsync(c => c.EmailId == emailId, cancellationToken);
        }

        public Task<Confirmation?> GetBySeedAsync(Guid seed, CancellationToken cancellationToken)
        {
            return dbContext.Confirmations.FirstOrDefaultAsync(c => c.Seed == seed, cancellationToken);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
