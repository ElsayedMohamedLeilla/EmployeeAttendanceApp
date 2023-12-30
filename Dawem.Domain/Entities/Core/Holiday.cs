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
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        public bool IsSpecifiedByYear { get; set; }

       
        public void JustifyStartEndDate()
        {
            if (!IsSpecifiedByYear)
            {
                DateTime tempStartDate = StartDate;
                DateTime tempEndDate = EndDate;
                StartDate =new DateTime(0001, tempStartDate.Month, tempStartDate.Day);
                EndDate = new DateTime(0001, tempEndDate.Month, tempEndDate.Day);
            }
        }

       public (string,string) CreateStartEndDate()
        {
            
            if (!IsSpecifiedByYear)
            {
                if(DateType.Equals(DateType.Hijri))
                {
                    HijriCalendar hijriCalendar = new HijriCalendar();
                    int currentHijriYear = hijriCalendar.GetYear(DateTime.UtcNow);
                    string startDate = new DateTime(currentHijriYear, StartDate.Month, StartDate.Day).ToShortDateString();
                    string endDate = new DateTime(currentHijriYear, EndDate.Month, EndDate.Day).ToShortDateString();
                    return (startDate, endDate);
                }
                else
                {
                     DateTime today = DateTime.UtcNow;
                    string startDate = new DateTime(today.Year, StartDate.Month, StartDate.Day).ToShortDateString();
                    string endDate = new DateTime(today.Year, EndDate.Month, EndDate.Day).ToShortDateString();
                    return (startDate, endDate);
                }
               
            }
            else
            {
                return (StartDate.ToShortDateString(), EndDate.ToShortDateString());
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
