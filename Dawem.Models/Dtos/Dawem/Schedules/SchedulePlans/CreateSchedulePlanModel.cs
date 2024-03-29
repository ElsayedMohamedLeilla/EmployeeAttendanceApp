using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Schedules.SchedulePlans
{
    public class CreateSchedulePlanModel
    {
        public ForType SchedulePlanType { get; set; }
        public int? EmployeeId { get; set; }
        public int? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime DateFrom { get; set; }
        public bool IsActive { get; set; }

        public void SetDefaults()
        {
            switch (SchedulePlanType)
            {
                case ForType.Employees:
                    GroupId = null;
                    DepartmentId = null;
                    break;
                case ForType.Groups:
                    EmployeeId = null;
                    DepartmentId = null;
                    break;
                case ForType.Departments:
                    EmployeeId = null;
                    GroupId = null;
                    break;
                default:
                    break;
            }
        }
    }
}
