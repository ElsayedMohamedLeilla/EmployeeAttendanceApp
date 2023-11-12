using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Attendances.Schedules
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
        public SchedulePlanType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
    }
}
