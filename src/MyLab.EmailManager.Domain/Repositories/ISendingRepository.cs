using MyLab.EmailManager.Domain.Entities;
using System.Linq.Expressions;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface ISendingRepository
    {
        void Add(Sending sending);

        Task<IList<Sending>> GetAsync(Expression<Func<Sending, bool>> specification, CancellationToken cancellationToken);

        Task SaveAsync(CancellationToken cancellationToken);
    }
}
