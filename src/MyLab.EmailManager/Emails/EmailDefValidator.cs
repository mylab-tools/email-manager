using FluentValidation;

namespace MyLab.EmailManager.Emails
{
    public class EmailDefValidator : AbstractValidator<EmailDefDto>
    {
        public EmailDefValidator()
        {
            RuleFor(d => d.Address).NotNull();
        }
    }
}
