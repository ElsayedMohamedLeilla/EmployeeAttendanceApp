using Newtonsoft.Json;


namespace Dawem.Helpers
{
    public static class Extensions
    {
        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default;
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

        public static object GetValObjDy(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }


        public static decimal Truncate(this decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;
        }

        public static float Truncate(this float value, int precision)
        {
            float step = (float)Math.Pow(10, precision);
            float tmp = (float)Math.Truncate(step * value);
            return tmp / step;
        }

        public static double Truncate(this double value, int precision)
        {
            double step = (double)Math.Pow(10, precision);
            double tmp = (double)Math.Truncate(step * value);
            return tmp / step;
        }


        public static string ConvertArabicNumbersToEnglish(this string number)
        {
            number = number.Replace("٠", "0");
            number = number.Replace("١", "1");
            number = number.Replace("٢", "2");
            number = number.Replace("٣", "3");
            number = number.Replace("٤", "4");
            number = number.Replace("٥", "5");
            number = number.Replace("٦", "6");
            number = number.Replace("٧", "7");
            number = number.Replace("٨", "8");
            number = number.Replace("٩", "9");

            return number;
        }


        public static IEnumerable<Type> GetInterfaces(this Type type, bool includeInherited)
        {
            if (includeInherited || type.BaseType == null)
                return type.GetInterfaces();
            else
                return type.GetInterfaces().Except(type.BaseType.GetInterfaces());
        }
    }
}
