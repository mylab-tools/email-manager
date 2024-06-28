using MediatR;

namespace MyLab.EmailManager.App.Features.GetEmail;

public record GetEmailQuery(Guid EmailId) : IRequest<EmailViewModel>;