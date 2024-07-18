using System.Collections.ObjectModel;
using AutoMapper;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using MyLab.EmailManager.Domain.ValueObjects;
using System;

namespace MyLab.EmailManager.App.Mapping
{
    public class SendingMappingProfile : Profile
    {
        public SendingMappingProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<DbSending, SendingViewModel>()
                .ForMember
                (
                    vm => vm.Selection,
                    opt =>
                    {
                        opt.ConvertUsing
                            (
                                StringLblArrToRoDictConverter.Instance, 
                                db => db.Selection
                            );
                    }
                )
                .ForMember
                (
                    vm => vm.TemplateArgs,
                    opt =>
                    {
                        opt.ConvertUsing
                        (
                            StringRoDictToRoDictConverter.Instance,
                            db => db.TemplateArgs
                        );
                    }
                );

            CreateMap<DbMessage, MessageViewModel>();
        }

        class StringRoDictToRoDictConverter : IValueConverter<string?, IReadOnlyDictionary<string, string>?>
        {
            public static readonly StringRoDictToRoDictConverter Instance = new();

            public IReadOnlyDictionary<string, string>? Convert(string? sourceMember, ResolutionContext context)
            {
                if (sourceMember == null) return null;

                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(sourceMember);
                return dict?.AsReadOnly();
            }
        }

        class StringLblArrToRoDictConverter : IValueConverter<string?, IReadOnlyDictionary<string, string>>
        {
            public static readonly StringLblArrToRoDictConverter Instance = new();

            public IReadOnlyDictionary<string, string> Convert(string? sourceMember, ResolutionContext context)
            {
                if (sourceMember == null)
                    return new Dictionary<string, string>();

                var serOpts = new JsonSerializerOptions(JsonSerializerOptions.Default);
                serOpts.Converters.Add(new JsonFilledStringConverter());

                var labels = JsonSerializer.Deserialize<EmailLabel[]>(sourceMember, serOpts);

                if (labels == null)
                    return new Dictionary<string, string>();

                return labels
                    .GroupBy(l => l.Name.Text)
                    .Select(g => new { GroupKey = g.Key, Items = g.ToArray() })
                    .Where(g => g.Items.Length > 0)
                    .ToDictionary(g => g.GroupKey, g => g.Items.Select(l => l.Value.Text).First())
                    .AsReadOnly();
            }
        }
        class JsonFilledStringConverter : JsonConverter<FilledString>
        {
            public override FilledString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var strVal = reader.GetString();

                return strVal != null ? new FilledString(strVal) : null;
            }

            public override void Write(Utf8JsonWriter writer, FilledString value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
