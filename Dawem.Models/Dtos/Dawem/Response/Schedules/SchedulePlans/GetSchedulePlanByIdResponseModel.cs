using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.SchedulePlans
{
    public class GetSchedulePlanByIdResponseModel
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int? EmployeeId { get; set; }
        public int? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public int Code { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public ForType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
    }
}
