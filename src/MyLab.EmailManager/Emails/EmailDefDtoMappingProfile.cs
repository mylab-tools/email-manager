using AutoMapper;
using MyLab.EmailManager.App.Features.CreateEmail;

namespace MyLab.EmailManager.Emails
{
    public class EmailDefDtoMappingProfile : Profile
    {
        public EmailDefDtoMappingProfile()
        {
            CreateMap<EmailDefDto, CreateEmailCommand>()
                .ForCtorParam("address", opt => opt.MapFrom(dto => dto.Address))
                .ForCtorParam("labels", opt => opt.MapFrom(dto => dto.Labels));
        }
    }
}
