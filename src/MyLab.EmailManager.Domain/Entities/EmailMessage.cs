using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities
{
    public class EmailMessage
    {
        public Guid Id { get; private set; }
        public Guid EmailId { get; private set; }
        public DateTime CreateDt { get; private set; }
        public DateTime? SendDt { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public FilledString Title { get; private set; }
        public TextContent Content { get; private set; }

        EmailMessage()
        {
            
        }

        public static EmailMessage New
            (
                Guid emailId,
                EmailAddress emailAddress, 
                FilledString title, 
                TextContent content
            )
        {
            if (emailAddress == null) throw new DomainException("The address is not defined");
            if (title == null) throw new DomainException("The title is not defined");
            if (content == null) throw new DomainException("The content is not defined");

            return new EmailMessage
            {
                EmailId = emailId,
                Title = title,
                Content = content,
                EmailAddress = emailAddress,
                CreateDt = DateTime.Now,
                Id = Guid.NewGuid()
            };
        }

        public void SetCurrentSendDateTime()
        {
            if (SendDt.HasValue)
                throw new DomainException("The message has already been sent");

            SendDt = DateTime.Now;
        }
    };
}
