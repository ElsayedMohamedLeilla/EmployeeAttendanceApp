using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Attendances
{
    public class GetEmployeeAttendancesCriteria : BaseCriteria
    {
        public int Month { get; set; }
        public int Year { get; set; }

    }
}
