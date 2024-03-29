using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Employees.Employees
{
    public class GetEmployeesCriteria : BaseCriteria
    {
        public int? DepartmentId { get; set; }
        public int? JobTitleId { get; set; }
        public int? ScheduleId { get; set; }
        public int? DirectManagerId { get; set; }
        public bool? WithoutCurrentEmployee { get; set; }
        public int? EmployeeNumber { get; set; }
        public FilterEmployeeStatus? Status { get; set; }
    }
}
