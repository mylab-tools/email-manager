using System.Collections.ObjectModel;

namespace MyLab.EmailManager.Domain.ValueObjects
{
    public class GenericMessageContent
    {
        public ReadOnlyDictionary<string, string> Args { get; }
        public FilledString PatternId { get; }

        public GenericMessageContent(FilledString patternId, IDictionary<FilledString, string> args)
        {
            PatternId = patternId;
            Args = args
                .ToDictionary
                (
                    t => t.Key.Text,
                    t => t.Value
                )
                .AsReadOnly();
        }
    }
}
