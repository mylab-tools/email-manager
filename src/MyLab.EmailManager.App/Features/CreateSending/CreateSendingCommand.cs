using MediatR;

namespace MyLab.EmailManager.App.Features.CreateSending
{
    public class CreateSendingCommand : IRequest<CreateSendingResponse>
    {
        public required IReadOnlyDictionary<string, string> Selection { get; init; }
        public required string Title { get; init; }
        public required string? SimpleContent { get; init; }
        public required string? TemplateId { get; init; }
        public required IReadOnlyDictionary<string, string>? TemplateArgs { get; init; }
    }
}
