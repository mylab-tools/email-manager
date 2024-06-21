using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Dto
{
    public class SendMessageDef
    {
        public required EmailLabel[] Selection { get; init; }
        public required FilledString Title { get; init; }
        public SimpleMessageContent? SimpleContent { get; init; }
        public GenericMessageContent? GenericContent { get; init; }
    }
}
