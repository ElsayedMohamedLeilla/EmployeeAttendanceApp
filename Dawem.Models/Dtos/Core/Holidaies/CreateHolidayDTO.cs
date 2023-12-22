using Dawem.Enums.Generals;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Dawem.Models.Dtos.Core.Holidaies
{
    public class CreateHolidayDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateType DateType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Notes { get; set; }

        [JsonIgnore]
        public int StartDay { get; set; }
        [JsonIgnore]
        public int EndDay { get; set; }
        [JsonIgnore]
        public int StartMonth { get; set; }
        [JsonIgnore]
        public int EndMonth { get; set; }
        [JsonIgnore]
        public int? StartYear { get; set; }
        [JsonIgnore]
        public int? EndYear { get; set; }


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

    }
}
