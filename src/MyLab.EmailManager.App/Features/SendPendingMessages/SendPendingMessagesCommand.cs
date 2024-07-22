using MediatR;

namespace MyLab.EmailManager.App.Features.SendPendingMessages;

public record SendPendingMessagesCommand : IRequest;