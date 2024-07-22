using FluentValidation;

namespace MyLab.EmailManager.Sendings
{
    public class SendingDefDtoValidator : AbstractValidator<SendingDefDto>
    {
        public SendingDefDtoValidator()
        {
            RuleFor(s => s.Title).NotEmpty();
            RuleFor(s => s.Selection).NotEmpty();
            RuleFor(s => s.TemplateId)
                .Must((s, val) => !string.IsNullOrEmpty(s.SimpleContent) || !string.IsNullOrEmpty(val))
                .WithMessage("templateId is required when simpleContent is not specified");
        }
    }
}
