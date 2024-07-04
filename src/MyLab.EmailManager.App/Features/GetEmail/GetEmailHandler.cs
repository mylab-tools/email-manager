using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.App.Common.ViewModels;
using MyLab.EmailManager.Infrastructure.EfModels;

namespace MyLab.EmailManager.App.Features.GetEmail;

public class GetEmailHandler(ReadDbContext dbContext) : IRequestHandler<GetEmailQuery, EmailViewModel?>
{
    public static readonly Expression<Func<DbEmail, EmailViewModel>> EmailConverter = dbEmail => new EmailViewModel
    {
        Id = dbEmail.Id,
        Address = dbEmail.Address,
        Labels = dbEmail.Labels
            .ToDictionary(l => l.Name, l => l.Value)
            .AsReadOnly(),
        Tail = Array.Empty<MessageViewModel>()
    };

    public Task<EmailViewModel?> Handle(GetEmailQuery getEmailQuery, CancellationToken cancellationToken)
    {
        return dbContext.Emails
            .Where(e => e.Id == getEmailQuery.EmailId)
            .Include(e => e.Labels)
            .Select(EmailConverter)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}