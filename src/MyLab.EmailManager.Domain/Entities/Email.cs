using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities
{
    public class Email(EmailAddress address)
    {
        public const string PrivateLabelsFieldName = nameof(_labels);

        List<EmailLabel> _labels = new();

        public DatedValue<bool> Deletion { get; private set; } = DatedValue<bool>.Unset;

        public EmailAddress Address { get; set; } = address;

        public IList<EmailLabel> Labels => _labels;

        public void UpdateLabels(IEnumerable<EmailLabel> newLabels)
        {
            _labels.Clear();
            _labels.AddRange(newLabels);
        }

        public void Delete()
        {
            if (Deletion.Value)
                throw new DomainException("An entity already has been deleted");

            Deletion = DatedValue<bool>.CreateSet(true);
        }
    }
}
