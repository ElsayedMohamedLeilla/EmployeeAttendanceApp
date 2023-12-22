using NodaTime;

namespace Dawem.Helpers
{
    public class DateConversionHelper
    {
        public static LocalDate ConvertToHijri(int year, int month, int day)
        {
            int daysInMonth = 29 + ((month + 2) % 3 + month / 2) % 2;
            int daysSoFar = (year - 1) * 354 + (month - 1) * 29 + day;
            int yearHijri = daysSoFar / 354 + 1;
            int dayOfYear = daysSoFar % 354;
            int monthHijri = (dayOfYear + 30) / 29;

            int dayOfMonth = dayOfYear % 29 == 0 ? 29 : dayOfYear % 29;

            return new LocalDate(yearHijri, monthHijri, dayOfMonth);
        }
    }
}
