using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLab.EmailManager.Infrastructure.JsonConverters;

namespace MyLab.EmailManager.Infrastructure.Db.EfConverters;

class ObjectToJsonStringConverter<T>() : ValueConverter<T, string>(ObjectToString, StringToObject)
    where T : class
{
    static Expression<Func<T?, string?>> ObjectToString =>
        obj => obj != null
            ? JsonSerializer.Serialize(obj, CustomJsonOptions.Options)
            : null;

    static Expression<Func<string?, T?>> StringToObject =>
        str => str != null
            ? JsonSerializer.Deserialize<T>(str, CustomJsonOptions.Options)
            : null;
}