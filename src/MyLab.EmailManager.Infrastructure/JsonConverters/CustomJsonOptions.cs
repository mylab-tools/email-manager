using System.Text.Json;

namespace MyLab.EmailManager.Infrastructure.JsonConverters;

class CustomJsonOptions
{
    public static readonly JsonSerializerOptions Options;

    static CustomJsonOptions()
    {
        Options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        Options.Converters.Add(new FilledStringJsonConverter());
    }
}