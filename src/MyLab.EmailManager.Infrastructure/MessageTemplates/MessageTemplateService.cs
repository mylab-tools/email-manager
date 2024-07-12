using Scriban;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public class MessageTemplateService(IMessageTemplateProvider tProvider) : IMessageTemplateService
{
    public async Task<string> CreateTextContentAsync(string templateId, IDictionary<string, string> args)
    {
        var templateText = await tProvider.ProvideAsync(templateId);

        var templateObject = Template.Parse(templateText);

        return await templateObject.RenderAsync(args);
    }
}