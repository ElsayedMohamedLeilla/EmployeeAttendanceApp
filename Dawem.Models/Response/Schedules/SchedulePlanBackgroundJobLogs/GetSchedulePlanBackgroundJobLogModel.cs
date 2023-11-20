using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.SchedulePlanBackgroundJobLogs
{
    public class GetSchedulePlanBackgroundJobLogModel
    {
        public int CompanyId { get; set; }
        public int SchedulePlanId { get; set; }
        public int ScheduleId { get; set; }
        public int? EmployeeId { get; set; }
        public int? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public SchedulePlanType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
    }
}
