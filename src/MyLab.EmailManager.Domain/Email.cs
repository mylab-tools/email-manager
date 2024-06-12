using Microsoft.VisualBasic.CompilerServices;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain
{
    public class Email(EmailAddress address)
    {
        List<EmailLabel> _labels = new List<EmailLabel>();

        public bool Deleted { get; private set; }

        public DateTime DeleteDt { get; private set; }

        public EmailAddress Address { get; set; } = address;

        public IList<EmailLabel> Labels => _labels;

        public void UpdateLabels(IEnumerable<EmailLabel> newLabels)
        {
            _labels.Clear();
            _labels.AddRange(newLabels);
        }

        public void Delete()
        {
            if (Deleted)
                throw new DomainException("An entity already has been deleted");

            Deleted = true;
            DeleteDt = DateTime.Now;
        }
    }
}
