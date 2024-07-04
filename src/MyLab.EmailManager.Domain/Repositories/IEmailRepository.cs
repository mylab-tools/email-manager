using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface IEmailRepository
    {
        void Add(Email email);
        Task<Email?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
