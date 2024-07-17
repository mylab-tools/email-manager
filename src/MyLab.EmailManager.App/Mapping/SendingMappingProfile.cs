using AutoMapper;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using System.Text.Json;

namespace MyLab.EmailManager.App.Mapping
{
    public class SendingMappingProfile : Profile
    {
        public SendingMappingProfile()
        {
            CreateMap<DbSending, SendingViewModel>()
                .ForMember
                (
                    vm => vm.Selection, 
                    opt =>
                    {
                        opt.MapFrom(s => s.Selection);
                        opt.ConvertUsing(StringToReadonlyDictionaryConverter.Instance);
                    });
        }

        class StringToReadonlyDictionaryConverter : IValueConverter<string, IReadOnlyDictionary<string, string>>
        {
            public static readonly StringToReadonlyDictionaryConverter Instance = new();

            public IReadOnlyDictionary<string, string> Convert(string sourceMember, ResolutionContext context)
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(sourceMember);
                return (dict ?? new Dictionary<string, string>()).AsReadOnly();
            }
        }
    }
}
