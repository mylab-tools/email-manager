using AutoMapper;
using MyLab.EmailManager.App.Features.CreateSending;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Common;

namespace MyLab.EmailManager.Sendings
{
    public class SendingDtoMappingProfile : Profile
    {
        public SendingDtoMappingProfile()
        {
            CreateMap<SendingDefDto, CreateSendingCommand>();

            CreateMap<SendingViewModel, SendingViewModelDto>()
                .ForMember
                (
                    dto => dto.SendingStatus, 
                    opt => opt.MapFrom(vm => vm.SendingStatus.Value)
                )
                .ForMember
                (
                    dto => dto.SendingStatusDt,
                    opt => opt.MapFrom(vm => vm.SendingStatus.DateTime)
                );
            CreateMap<MessageViewModel, MessageViewModelDto>()
                .ForMember
                (
                    dto => dto.SendingStatus,
                    opt => opt.MapFrom(vm => vm.SendingStatus.Value)
                )
                .ForMember
                (
                    dto => dto.SendingStatusDt,
                    opt => opt.MapFrom(vm => vm.SendingStatus.DateTime)
                );
        }
    }
}
