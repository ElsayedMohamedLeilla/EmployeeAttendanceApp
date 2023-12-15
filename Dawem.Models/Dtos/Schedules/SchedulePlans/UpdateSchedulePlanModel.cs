using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Schedules.SchedulePlans
{
    public class UpdateSchedulePlanModel
    {
        public int Id { get; set; }
        public ForType SchedulePlanType { get; set; }
        public int? EmployeeId { get; set; }
        public int? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime DateFrom { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public void SetDefaults()
        {
            switch (SchedulePlanType)
            {
                case SchedulePlanType.Employees:
                    GroupId = null;
                    DepartmentId = null;
                    break;
                case SchedulePlanType.Groups:
                    EmployeeId = null;
                    DepartmentId = null;
                    break;
                case SchedulePlanType.Departments:
                    EmployeeId = null;
                    GroupId = null;
                    break;
                default:
                    break;
            }
        }
    }
}
