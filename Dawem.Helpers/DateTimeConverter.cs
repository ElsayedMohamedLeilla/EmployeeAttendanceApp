using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dawem.Helpers
{
    public class DateTimeConverter : DateTimeConverterBase
    {
        public DateTimeConverter()
        {
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var tt = reader.TokenType;
            if (tt == JsonToken.String)
            {
                DateTime date;
                if (DateTime.TryParseExact((string)reader.Value, new[] { "yyyy/MM/dd", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date)
                    || DateTime.TryParseExact((string)reader.Value, "yyyy-MM-dd'T'HH:mm", System.Globalization.CultureInfo.InvariantCulture,
                   System.Globalization.DateTimeStyles.None, out date))
                {
                    return date;
                }

                return reader.Value;
            }

            return reader.Value;


        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var myDt = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Unspecified);
            try
            {
                value = myDt;
            }
            catch (Exception ex)
            {
            }
            writer.WriteValue(value);
            return;


        }
    }
}