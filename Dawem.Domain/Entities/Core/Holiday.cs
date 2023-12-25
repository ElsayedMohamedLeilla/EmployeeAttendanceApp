using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(Holiday) + LeillaKeys.S)]
    public class Holiday : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public DateType DateType { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        [NotMapped]
        public DateOnly StartDate { get; set; }
        [NotMapped]
        public DateOnly EndDate { get; set; }
        public bool IsSpecifiedByYear { get; set; }

        public DateTime ToGregorianDate(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }


        public DateTime ToHijriDate(int year, int month, int day)
        {
            HijriCalendar hijriCalendar = new();
            return hijriCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }


        public string GetStartDateAsString(int? year)
        {
            if (DateType == DateType.Hijri)
            {
                HijriCalendar hijriCalendar = new HijriCalendar();
                int currentHijriYear = hijriCalendar.GetYear(DateTime.UtcNow);
                return currentHijriYear + "-" + StartMonth + "-" + StartDay;
            }
            else
            {
                int gyear = StartYear ?? DateTime.UtcNow.Year;
                return gyear + "-" + StartMonth + "-" + StartDay;
            }
        }
        public string GetEndDateAsString(int? year)
        {
            if (DateType == DateType.Hijri)
            {
                HijriCalendar hijriCalendar = new HijriCalendar();
                int currentHijriYear = hijriCalendar.GetYear(DateTime.UtcNow);
                    return currentHijriYear + "-" + EndMonth + "-" + EndDay;
              
            }
            else
            {
                int gyear = EndYear ?? DateTime.UtcNow.Year;
                return gyear + "-" + EndMonth + "-" + EndDay;
            }
        }

        public void AssignStartEndDayMonthYear()
        {
            StartDay = StartDate.Day;
            StartMonth = StartDate.Month;
            EndDay = EndDate.Day;
            EndMonth = EndDate.Month;
            if (IsSpecifiedByYear)
            {
                StartYear = StartDate.Year;
                EndYear = EndDate.Year;
            }
        }

       

        public LocalDate GetHijriDate(int hijriYear, int hijriMonth, int hijriDay)
        {
            LocalDate Date = ConvertToHijri(hijriYear, hijriMonth, hijriDay);
            return Date;
        }

        public static LocalDate ConvertToHijri(int year, int month, int day)
        {
            LocalDate Date;
            int daysInMonth = 29 + ((month + 2) % 3 + month / 2) % 2;
            int daysSoFar = (year - 1) * 354 + (month - 1) * 29 + day;
            int yearHijri = daysSoFar / 354 + 1;
            int dayOfYear = daysSoFar % 354;
            int monthHijri = (dayOfYear + 30) / 29;
            int dayOfMonth = dayOfYear % 29 == 0 ? 29 : dayOfYear % 29;
            Date = new LocalDate(yearHijri, monthHijri, dayOfMonth);
            return Date;
        }






















    }
}
