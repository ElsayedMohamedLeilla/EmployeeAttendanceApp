using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Requests.Tasks
{
    public class EmployeeGetRequestTasksCriteria : BaseCriteria
    {
        public int Month { get; set; }
        public int Year { get; set; }

    }
}
