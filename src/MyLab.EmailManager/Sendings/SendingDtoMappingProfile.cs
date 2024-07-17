using AutoMapper;
using MyLab.EmailManager.App.Features.CreateSending;
using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.Sendings
{
    public class SendingDtoMappingProfile : Profile
    {
        public SendingDtoMappingProfile()
        {
            CreateMap<SendingDefDto, CreateSendingCommand>();

            CreateMap<SendingViewModel, SendingViewModelDto>();
        }
    }
}
