using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class GetCompaniesCriteria : BaseCriteria
    {
        public int? CountryId { get; set; }
    }
}
