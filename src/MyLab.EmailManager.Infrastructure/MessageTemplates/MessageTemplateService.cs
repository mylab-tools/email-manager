using MyLab.EmailManager.Domain.ValueObjects;
using Scriban;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public class MessageTemplateService(IMessageTemplateProvider tProvider) : IMessageTemplateService
{
    public async Task<TextContent> CreateTextContentAsync(string templateId, TemplateContext tCtx, CancellationToken cancellationToken)
    {
        var templateText = await tProvider.ProvideAsync(templateId, cancellationToken);

        if (templateText == null)
            throw new InvalidOperationException("Template content is null");

        var templateObject = Template.Parse(templateText.Text);

        var content = await templateObject.RenderAsync(tCtx);

        return templateText with { Text = content };
    }
}