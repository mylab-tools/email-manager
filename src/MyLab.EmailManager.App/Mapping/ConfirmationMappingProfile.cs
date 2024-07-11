using AutoMapper;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace MyLab.EmailManager.App.Mapping
{
    public class ConfirmationMappingProfile : Profile
    {
        public ConfirmationMappingProfile()
        {
            CreateMap<DbConfirmation, ConfirmationState>()
                .ForMember
                (
                    vm => vm.Confirmed,
                    opt => opt.MapFrom
                        (
                            db => db.Step == (int)ConfirmationStep.Confirmed
                        )
                )
                .ForMember
                (
                    vm => vm.Step,
                    opt => opt.MapFrom
                        (
                            db => (ConfirmationStep)db.Step
                        )
                );
        }
    }
}
