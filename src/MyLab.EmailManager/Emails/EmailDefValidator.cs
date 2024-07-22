using FluentValidation;

namespace MyLab.EmailManager.Emails
{
    public class EmailDefValidator : AbstractValidator<EmailDefDto>
    {
        public static readonly EmailDefValidator Instance = new EmailDefValidator();
        EmailDefValidator()
        {
            RuleFor(d => d.Address).NotNull();
        }
    }
}
