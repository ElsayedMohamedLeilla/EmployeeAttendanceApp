using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Employees.Departments
{
    public class GetDepartmentsCriteria : BaseCriteria
    {
        public bool IsBaseParent { get; set; }
        public int? ParentId { get; set; }
    }
}
