using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulePlanInfoResponseModel
    {
        public int ScheduleName { get; set; }
        public string EmployeeName { get; set; }
        public string GroupName { get; set; }
        public string DepartmentName { get; set; }
        public int Code { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public SchedulePlanType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
    }
}
