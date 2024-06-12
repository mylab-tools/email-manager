using Microsoft.VisualBasic.CompilerServices;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain
{
    public class Email(EmailAddress address)
    {
        List<EmailLabel> _labels = new List<EmailLabel>();

        public EmailAddress Address { get; set; } = address;

        public IList<EmailLabel> Labels => _labels;

        public void UpdateLabels(IEnumerable<EmailLabel> newLabels)
        {
            _labels.Clear();
            _labels.AddRange(newLabels);
        }
    }
}
