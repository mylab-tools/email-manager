using MediatR;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Infrastructure;

namespace MyLab.EmailManager.App.Features.GetEmail;

public class GetEmailHandler(AppDbContext dbContext) : IRequestHandler<GetEmailQuery, EmailViewModel>
{
    public async Task<EmailViewModel> Handle(GetEmailQuery getEmailQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //dbContext.Database.SqlQuery<EmailViewModel>();
    }
}