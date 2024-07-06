using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface IEmailRepository
    {
        Task AddAsync(Email email, CancellationToken cancellationToken);
        Task<Email?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
