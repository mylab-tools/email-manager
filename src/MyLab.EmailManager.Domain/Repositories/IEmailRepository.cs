using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface IEmailRepository
    {
        Task AddAsync(Email email);

        Email Get(Guid id);
    }
}
