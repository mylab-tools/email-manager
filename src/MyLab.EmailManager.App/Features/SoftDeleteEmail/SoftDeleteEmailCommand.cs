using MediatR;

namespace MyLab.EmailManager.App.Features.SoftDeleteEmail;

public record SoftDeleteEmailCommand(Guid EmailId) : IRequest;