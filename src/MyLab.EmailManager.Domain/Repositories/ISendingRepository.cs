using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface ISendingRepository
    {
        void Add(Sending sending);

        Task<IList<Sending>> GetActiveAsync(CancellationToken cancellationToken);

        Task SaveAsync(CancellationToken cancellationToken);
    }
}
