namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public class MessageTemplateProvider(string basePath) : IMessageTemplateProvider
{
    private readonly string _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));

    public async Task<string> ProvideAsync(string templateId)
    {
        if (string.IsNullOrWhiteSpace(templateId))
            throw new InvalidTemplateIdException(templateId);

        var templateFilePath = Path.Combine(_basePath, templateId, ".sbn");

        if (File.Exists(templateFilePath))
            throw new TemplateNotFoundException(templateId);

        return await File.ReadAllTextAsync(templateFilePath);
    }
}