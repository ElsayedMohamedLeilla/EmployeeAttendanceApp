using Dawem.Enums.Generals;
using Dawem.Translations;

namespace Dawem.Helpers
{
    public class DateHelper
    {
        public static List<WeekDay> GetWeekDaysBetweenTwoDates(DateTime fromDate, DateTime toDate)
        {
            int daysDiff = (toDate - fromDate).Days;
            var weekDays = Enumerable.Range(0, daysDiff + 1) // +1 because you want to include start and end date
                .Select(d => (WeekDay)fromDate.AddDays(d).DayOfWeek)
                .ToList();

            return weekDays;
        }
        public static List<DateTime> GetDatesBetweenTwoDates(DateTime fromDate, DateTime toDate)
        {
            var allDates = Enumerable.Range(0, 1 + toDate.Subtract(fromDate).Days)
            .Select(offset => fromDate.AddDays(offset))
            .ToList();

            return allDates;
        }
        public static DateTime GetLocalDateTime(string timeZoneId)
        {
            var localDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, timeZoneId).DateTime;
            return localDateTime;
        }
        public static DateTime GetUTSDateTime(DateTime localDate, string timeZoneId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var UTCDateTime = TimeZoneInfo.ConvertTimeToUtc(localDate, timeZoneInfo);
            return UTCDateTime;
        }
        public static string TranslateAmAndPm(string AmOrPm, string lang)
        {
            return LeillaKeys.Space + TranslationHelper.GetTranslation(AmOrPm, lang);
        }
    }
}
