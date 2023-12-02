using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Attendances
{
    public class GetEmployeeAttendancesForWebAdminCriteria : BaseCriteria
    {
        public int? EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public int? EmployeeNumber { get; set; }
    }
}
