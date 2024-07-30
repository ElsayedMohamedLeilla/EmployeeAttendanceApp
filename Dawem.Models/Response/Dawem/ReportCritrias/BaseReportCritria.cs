using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.ReportCritrias
{
    public class BaseReportCritria
    {
        public string FreeText { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? DepartmentId { get; set; }
        public int? ZoneId { get; set; }
        public int? JobTitleID { get; set; }
        public ExportFormat ExportFormat { get; set; }
        public int? OverTimeFrom { get; set; }
        public int? OverTimeTo { get; set; }
        public bool? WithoutPermision { get; set; }
        public ReportNotifyWay? NotifiyWay { get; set; }
        public int? AllowedTimeWithMinutesFrom { get; set; }
        public int? AllowedTimeWithMinutesTo { get; set; }
        public int? NoOfRequiredEmployeeFrom { get; set; }
        public int? NoOfRequiredEmployeeTo { get; set; }
        public int? PercentageOfDoneFrom { get; set; }
        public int? PercentageOfDoneTo { get; set; }
        public DoneStatus? DoneStatus { get; set; }
        public StatisticsReportOrderBy? statisticsReportOrderBy { get; set; }

    }
}
