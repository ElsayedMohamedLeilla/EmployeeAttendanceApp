using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Attendances
{
    public class EmployeeGetRequestTasksCriteria : BaseCriteria
    {
        public int Month { get; set; }
        public int Year { get; set; }

    }
}
