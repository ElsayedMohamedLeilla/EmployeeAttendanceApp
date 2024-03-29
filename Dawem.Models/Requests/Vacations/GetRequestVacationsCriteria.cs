using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Requests.Vacations
{
    public class GetRequestVacationsCriteria : BaseCriteria
    {
        public int? VacationTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
