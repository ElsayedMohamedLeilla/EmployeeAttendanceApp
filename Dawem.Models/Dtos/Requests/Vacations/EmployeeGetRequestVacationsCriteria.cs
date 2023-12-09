using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Requests.Vacations
{
    public class EmployeeGetRequestVacationsCriteria : BaseCriteria
    {
        public int? VacationTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
