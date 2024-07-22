using MediatR;
using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.App.Features.GetEmail;

public record GetEmailQuery(Guid EmailId) : IRequest<EmailViewModel?>;