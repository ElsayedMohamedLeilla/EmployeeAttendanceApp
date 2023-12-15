using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Provider;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Dawem.Domain.Entities.Core
{
    [Table("Holida" + LeillaKeys.IES)]
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
        public DateTime StartDate { get; set; }
        [NotMapped]
        public DateTime EndDate { get; set; }

        public DateTime ToGregorianDate(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }


        public DateTime ToHijriDate(int year, int month, int day)
        {
            HijriCalendar hijriCalendar = new HijriCalendar();
            return hijriCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }

        
        public string GetStartDate()
        {
            if (DateType == DateType.Hijri)
            {
                HijriCalendar hijriCalendar = new HijriCalendar();
                return hijriCalendar.ToDateTime(DateTime.Now.Year, StartMonth, StartDay, 0, 0, 0, 0).ToShortDateString();
            }
            else
            {
                return new DateTime(StartYear ?? DateTime.Now.Year, StartMonth, StartDay).ToShortDateString();
            }
        }
        public string GetEndDate()
        {
            if (DateType == DateType.Hijri)
            {
                HijriCalendar hijriCalendar = new();
                return hijriCalendar.ToDateTime(DateTime.Now.Year, EndMonth, EndDay, 0, 0, 0, 0).ToShortDateString();
            }
            else
            {
                return new DateTime(EndYear ?? DateTime.Now.Year, EndMonth, EndDay).ToShortDateString();
            }
        }

        public void AssignStartEndDayMonthYear()
        {
            StartDay = StartDate.Day;
            StartMonth = StartDate.Month;
            EndDay = EndDate.Day;
            EndMonth = EndDate.Month;
            if (DateType == DateType.Gregorian)
            {
                StartYear = StartDate.Year;
                EndYear = EndDate.Year;
            }
        }






















    }
}
