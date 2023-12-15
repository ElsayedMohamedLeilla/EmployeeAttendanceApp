using System.Globalization;

namespace Dawem.Helpers
{
    public static class OthersHelper
    {
        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
        public static bool IsValidLatitude(this double latitude)
            => -90 <= latitude && latitude <= 90;

        public static bool IsValidLongitude(this double longitude)
            => -180 <= longitude && longitude <= 180;

        public static bool IsValidLatitude(this decimal latitude)
        {
            return -90 <= latitude && latitude <= 90;
        }

        public static bool IsValidLongitude(this decimal longitude)
            =>  -180 <= longitude && longitude <= 180;

        public static bool ValidateIPv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}
