using AutoMapper;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace MyLab.EmailManager.App.Mapping
{
    public class EmailMappingProfile : Profile
    {
        public EmailMappingProfile()
        {
            CreateMap<DbEmail, EmailViewModel>()
                .ForMember
                (
                    vm => vm.Labels,
                    opt => opt.MapFrom
                        (
                            e => e.Labels.ToDictionary
                                (
                                    l => l.Name,
                                    l => l.Value
                                ).AsReadOnly()
                        )
                );
        }
    }
}
