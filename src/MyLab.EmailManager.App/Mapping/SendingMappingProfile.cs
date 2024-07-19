using AutoMapper;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MyLab.EmailManager.Domain.ValueObjects;
using static System.Net.Mime.MediaTypeNames;

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
                )
                .ForMember
                (
                    vm => vm.SendingStatus,
                    opt => opt.MapFrom<DbSendingSendingStatusValueResolver>()
                );

            CreateMap<DbMessage, MessageViewModel>()
                .ForMember
                (
                    vm => vm.SendingStatus,
                    opt => opt.MapFrom<DbMessageSendingStatusValueResolver>()
                );
        }

        public class DbMessageSendingStatusValueResolver : SendingStatusValueResolver<DbMessage, MessageViewModel>
        {

            protected override string GetSourceSendingStatus(DbMessage source)
            {
                return source.SendingStatus;
            }

            protected override DateTime GetSourceSendingStatusDt(DbMessage source)
            {
                return source.SendingStatusDt;
            }
        }

        public class DbSendingSendingStatusValueResolver : SendingStatusValueResolver<DbSending, SendingViewModel>
        {
            protected override string GetSourceSendingStatus(DbSending source)
            {
                return source.SendingStatus;
            }

            protected override DateTime GetSourceSendingStatusDt(DbSending source)
            {
                return source.SendingStatusDt;
            }
        }

        public abstract class SendingStatusValueResolver<TSource, TDest> : IValueResolver<TSource, TDest, DatedValue<SendingStatus>>
        {
            public DatedValue<SendingStatus> Resolve(TSource source, TDest destination, DatedValue<SendingStatus> destMember, ResolutionContext context)
            {
                return DatedValue<SendingStatus>.CreateSet
                (
                    Enum.Parse<SendingStatus>(GetSourceSendingStatus(source), ignoreCase: true),
                    GetSourceSendingStatusDt(source)
                );
            }

            protected abstract string GetSourceSendingStatus(TSource source);
            protected abstract DateTime GetSourceSendingStatusDt(TSource source);
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
