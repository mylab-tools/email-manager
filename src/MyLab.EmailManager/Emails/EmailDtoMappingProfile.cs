using AutoMapper;
using MyLab.EmailManager.App.Features.CreateEmail;
using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.Emails
{
    public class EmailDtoMappingProfile : Profile
    {
        public EmailDtoMappingProfile()
        {
            CreateMap<EmailDefDto, CreateEmailCommand>()
                .ForCtorParam("address", opt => opt.MapFrom(dto => dto.Address))
                .ForCtorParam("labels", opt => opt.MapFrom(dto => dto.Labels));

            CreateMap<EmailViewModel, EmailViewModelDto>();
        }
    }
}
