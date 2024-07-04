using MediatR;

namespace MyLab.EmailManager.App.Features.RemoveEmail;

public record RemoveEmailCommand(Guid EmailId) : IRequest;