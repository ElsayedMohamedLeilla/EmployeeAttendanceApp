using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class GetEmployeeAttendancesCriteria : BaseCriteria
    {
        public int Month { get; set; }
        public int Year { get; set; }

    }
}
