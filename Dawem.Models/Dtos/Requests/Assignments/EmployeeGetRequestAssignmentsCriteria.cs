using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Attendances
{
    public class EmployeeGetRequestAssignmentsCriteria : BaseCriteria
    {
        public int Month { get; set; }
        public int Year { get; set; }

    }
}
