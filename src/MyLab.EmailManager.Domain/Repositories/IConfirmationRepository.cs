using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface IConfirmationRepository
    {
        Task AddAsync(Confirmation confirmation, CancellationToken cancellationToken);
        Task<Confirmation?> GetAsync(Guid emailId, CancellationToken cancellationToken);
        Task<Confirmation?> GetBySeedAsync(Guid seed, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
