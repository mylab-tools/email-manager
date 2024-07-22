using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Infrastructure.Db;

namespace MyLab.EmailManager.Infrastructure.Repositories
{
    public class EmailRepository(DomainDbContext dbContext) : IEmailRepository
    {
        public async Task AddAsync(Email email, CancellationToken cancellationToken)
        {
            await dbContext.Emails.AddAsync(email, cancellationToken);
        }

        public Task<Email?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return dbContext.Emails.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public Task SaveAsync(CancellationToken cancellationToken)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
