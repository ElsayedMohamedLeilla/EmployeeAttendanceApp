using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class CreateSchedulePlanModel
    {
        public SchedulePlanType SchedulePlanType { get; set; }
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
                case SchedulePlanType.Employee:
                    GroupId = null;
                    DepartmentId = null;
                    break;
                case SchedulePlanType.Group:
                    EmployeeId = null;
                    DepartmentId = null;
                    break;
                case SchedulePlanType.Department:
                    EmployeeId = null;
                    GroupId = null;             
                    break;
                default:
                    break;
            }
        }
    }
}
