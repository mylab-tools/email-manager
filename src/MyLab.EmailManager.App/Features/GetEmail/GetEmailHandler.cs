using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.App.Exceptions;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace MyLab.EmailManager.App.Features.GetEmail;

public class GetEmailHandler(ReadDbContext dbContext, IMapper mapper) : IRequestHandler<GetEmailQuery, EmailViewModel?>
{
    public async Task<EmailViewModel?> Handle(GetEmailQuery getEmailQuery, CancellationToken cancellationToken)
    {
        var storedEmail = await dbContext.Emails
            .Where(e => e.Id == getEmailQuery.EmailId && !e.Deleted)
            .Include(e => e.Labels)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return storedEmail != null
            ? mapper.Map<EmailViewModel>(storedEmail)
            : null;
    }
}