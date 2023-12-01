using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Attendances
{
    public class GetEmployeeAttendancesForWebAdminCriteria : BaseCriteria
    {

        public string EmployeeName { get; set; }
        public DateOnly Date { get; set; }


    }
}
