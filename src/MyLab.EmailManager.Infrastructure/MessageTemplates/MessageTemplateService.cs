using MyLab.EmailManager.Infrastructure.Messaging;
using Scriban;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public class MessageTemplateService(IMessageTemplateProvider tProvider) : IMessageTemplateService
{
    public async Task<TextContent> CreateTextContentAsync(string templateId, IReadOnlyDictionary<string, string>? args)
    {
        var templateText = await tProvider.ProvideAsync(templateId);

        if (templateText == null)
            throw new InvalidOperationException("Template content is null");

        var templateObject = Template.Parse(templateText.Content);

        var content = await templateObject.RenderAsync(args);

        return templateText with { Content = content };
    }
}