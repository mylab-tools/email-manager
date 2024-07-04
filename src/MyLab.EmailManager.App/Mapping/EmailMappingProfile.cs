using AutoMapper;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace MyLab.EmailManager.App.Mapping
{
    class EmailMappingProfile : Profile
    {
        public EmailMappingProfile()
        {
            CreateMap<DbEmail, Email>();
        }
    }
}
