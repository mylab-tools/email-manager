using Microsoft.Extensions.Options;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public class MessageTemplateProvider(TemplateOptions options) : IMessageTemplateProvider
{
    public MessageTemplateProvider(IOptions<TemplateOptions> opts)
        :this(opts.Value)
    {
        
    }

    public async Task<TextContent> ProvideAsync(string templateId)
    {
        if (string.IsNullOrWhiteSpace(templateId))
            throw new InvalidTemplateIdException(templateId);

        var textTemplateFilePath = Path.Combine(options.BasePath, templateId, ".sbn-txt");
        var htmlTemplateFilePath = Path.Combine(options.BasePath, templateId, ".sbn-htm");

        var textTemplateFileExists = File.Exists(textTemplateFilePath);
        var htmlTemplateFileExists = File.Exists(htmlTemplateFilePath);


        if (!textTemplateFileExists && ! htmlTemplateFileExists)
            throw new TemplateNotFoundException(templateId);

        var templateFilePath = htmlTemplateFileExists
            ? htmlTemplateFilePath
            : textTemplateFilePath;

        var content = await File.ReadAllTextAsync(templateFilePath);

        return new (content, htmlTemplateFileExists);
    }
}