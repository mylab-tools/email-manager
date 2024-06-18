using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Dto
{
    public class SendMessageDef
    {
        public required EmailLabel[] Selection { get; init; }
        public required FilledString Title { get; init; }
        public SimpleMessageDef? SimpleMsg { get; init; }
        public GenericMessageDef? GenericMsg { get; init; }
    }
}
