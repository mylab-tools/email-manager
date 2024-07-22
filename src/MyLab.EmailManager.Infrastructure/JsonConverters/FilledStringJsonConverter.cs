using System.Text.Json;
using System.Text.Json.Serialization;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Infrastructure.JsonConverters
{
    class FilledStringJsonConverter : JsonConverter<FilledString>
    {
        public override FilledString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var readStr = reader.GetString();
            return readStr != null ? new FilledString(readStr) : null;
        }

        public override void Write(Utf8JsonWriter writer, FilledString value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Text);
        }
    }
}
