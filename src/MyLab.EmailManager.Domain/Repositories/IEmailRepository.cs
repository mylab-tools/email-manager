using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Domain.Repositories
{
    public interface IEmailRepository
    {
        Guid Add(Email email);

        Email Get(Guid id);
    }
}
