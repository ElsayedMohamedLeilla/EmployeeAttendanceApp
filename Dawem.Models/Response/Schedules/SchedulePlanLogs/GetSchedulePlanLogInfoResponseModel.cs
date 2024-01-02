using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.SchedulePlanBackgroundJobLogs
{
    public class GetSchedulePlanLogInfoResponseModel
    {
        public int Code { get; set; }
        public string ScheduleName { get; set; }
        public string EmployeeName { get; set; }
        public string GroupName { get; set; }
        public string DepartmentName { get; set; }
        public string Notes { get; set; }
        public string SchedulePlanTypeName { get; set; }
        public ForType SchedulePlanType { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime ScheduleDateFrom { get; set; }
        public int EmployeesNumberAppliedOn { get; set; }
        public List<GetSchedulePlanLogEmployeeInfoModel> EmployeesAppliedOn { get; set; }
    }
}
