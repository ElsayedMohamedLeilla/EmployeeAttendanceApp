using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Reports.AttendanceSummaryReport
{
    public class AttendanceSummaryHelperModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<WeekDay> WeekDays { get; set; }
    }
}
