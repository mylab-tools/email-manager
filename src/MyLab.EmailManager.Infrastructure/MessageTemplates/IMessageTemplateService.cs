using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates
{
    public interface IMessageTemplateService
    {
        Task<TextContent> CreateTextContentAsync(string templateId, TemplateContext tCtx, CancellationToken cancellationToken);
    }

    public record TemplateContext(IReadOnlyDictionary<string, string>? Args, IReadOnlyDictionary<string, string>? Email);
}
