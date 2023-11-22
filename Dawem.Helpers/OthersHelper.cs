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
            => double.TryParse(latitude.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var l)
        && -90 <= l && l <= 90;

        public static bool IsValidLongitude(this double longitude)
            => double.TryParse(longitude.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var l)
                && -180 <= l && l <= 180;
    }
}
