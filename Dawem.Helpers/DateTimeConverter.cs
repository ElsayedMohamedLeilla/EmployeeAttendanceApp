using Newtonsoft.Json;
using System.Globalization;

namespace Dawem.Helpers
{
    public class MultiFormatDateConverter : JsonConverter
    {
        public string[] DateTimeFormats =
            new string [] { "dd-MM-yyyy","yyyy-MM-dd",
                "MM-dd-yyyy", "dd/MM/yyyy","yyyy/MM/dd",
                "MM/dd/yyyy" , "yyyy-dd-MM",
                "yyyy-MM-dd HH:mm:ss","yyyyMMddTHHmmssZ",
                "yyyy-MM-ddTHH:mm",
                "dd-MM-yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss",
                "yyyy/MM/dd HH:mm:ss" , "dd/MM/yyyy HH:mm:ss",
                "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd HH:mm",
                "dd-MM-yyyy HH:mm", "MM-dd-yyyy HH:mm",
                "yyyy/MM/dd HH:mm" , "dd/MM/yyyy HH:mm",
                "MM/dd/yyyy HH:mm",
                "yyyy-MM-ddTHH:mm:ss.fffK"
                };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            string dateString = reader.Value.ToString();
            if (dateString == null)
            {
                if (objectType == typeof(DateTime?))
                    return null;

                throw new JsonException("Unable to parse null as a date.");
            }
            DateTime date;
            if (DateTime.TryParseExact(dateString, DateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date;
            throw new JsonException("Unable to parse \"" + dateString + "\" as a date.");
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}