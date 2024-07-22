using AutoMapper;
using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.Confirmations;

public class ConfirmationStateDtoMappingProfile : Profile
{
    public ConfirmationStateDtoMappingProfile()
    {
        CreateMap<ConfirmationState, ConfirmationStateDto>();
    }
}