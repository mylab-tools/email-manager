using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface ISendingRepository
    {
        void Add(Sending sending);

        Task SaveAsync(CancellationToken cancellationToken);
    }
}
