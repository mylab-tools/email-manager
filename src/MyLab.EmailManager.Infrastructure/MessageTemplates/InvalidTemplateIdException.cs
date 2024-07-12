namespace MyLab.EmailManager.Infrastructure.MessageTemplates;

public class InvalidTemplateIdException(string templateId) : Exception("Invalid template ID")
{
    public string TemplateId { get; } = templateId;
}