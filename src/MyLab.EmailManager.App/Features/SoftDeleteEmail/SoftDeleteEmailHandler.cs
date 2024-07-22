using MediatR;
using MyLab.EmailManager.App.Exceptions;
using MyLab.EmailManager.Domain.Repositories;

namespace MyLab.EmailManager.App.Features.SoftDeleteEmail
{
    public class SoftDeleteEmailHandler(IEmailRepository emailRepository) : IRequestHandler<SoftDeleteEmailCommand>
    {
        public async Task Handle(SoftDeleteEmailCommand request, CancellationToken cancellationToken)
        {
            var email = await emailRepository.GetAsync(request.EmailId, cancellationToken);

            if (email == null)
                throw new NotFoundException("Email not found");

            email.Delete();
            await emailRepository.SaveAsync(cancellationToken);
        }
    }
}
