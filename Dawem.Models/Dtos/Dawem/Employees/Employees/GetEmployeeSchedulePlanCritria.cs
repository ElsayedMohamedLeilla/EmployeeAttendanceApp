using Dawem.Models.Criteria;

namespace Dawem.Models.DTOs.Dawem.Employees.Employees
{
    public class GetEmployeeSchedulePlanCritria 
    {
        public int EmployeeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
