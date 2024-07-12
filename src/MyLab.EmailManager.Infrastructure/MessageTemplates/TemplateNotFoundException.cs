using Scriban;

namespace MyLab.EmailManager.Infrastructure.MessageTemplates
{
    public class TemplateNotFoundException(string templateId) : Exception("Template not found")
    {
        public string TemplateId { get; } = templateId;
    }
}
