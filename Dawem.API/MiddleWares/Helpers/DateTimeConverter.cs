using Newtonsoft.Json;
using System.Globalization;

namespace Dawem.API.MiddleWares
{
    public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateTimeOffset.ParseExact(reader.Value.ToString(),
                     "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }
}
