using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Requests.Assignments
{
    public class EmployeeGetRequestAssignmentsCriteria : BaseCriteria
    {
        public int Month { get; set; }
        public int Year { get; set; }

    }
}
