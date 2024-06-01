namespace Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport
{
    public class BriefingSummonsInPeriodCritria
    {
        public int EmployeeID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int DepartmentID { get; set; }
        public int ZoneID { get; set; }
        public int JobTitleID { get; set; }
        public int CompanyID { get; set; }
        public int NotifiyWay { get; set; }
        public int AllowedTimeWithMinutesFrom { get; set; }
        public int AllowedTimeWithMinutesTo { get; set; }
        public int NoOfRequiredEmployeeFrom { get; set; }
        public int NoOfRequiredEmployeeTo { get; set; }
        public int PercentageOfDoneFrom { get; set; }
        public int PercentageOfDoneTo { get; set; }

    }
}
