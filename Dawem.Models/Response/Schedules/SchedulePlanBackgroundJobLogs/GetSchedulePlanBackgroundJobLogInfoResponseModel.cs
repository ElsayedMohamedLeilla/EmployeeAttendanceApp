using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.SchedulePlanBackgroundJobLogs
{
    public class GetSchedulePlanBackgroundJobLogInfoResponseModel
    {
        public int Code { get; set; }
        public string ScheduleName { get; set; }
        public string EmployeeName { get; set; }
        public string GroupName { get; set; }
        public string DepartmentName { get; set; }
        public string Notes { get; set; }
        public string SchedulePlanTypeName { get; set; }
        public SchedulePlanType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
        public int EmployeesNumberAppliedOn { get; set; }
        public List<GetSchedulePlanBackgroundJobLogEmployeeInfoModel> EmployeesAppliedOn { get; set; }
    }
}
