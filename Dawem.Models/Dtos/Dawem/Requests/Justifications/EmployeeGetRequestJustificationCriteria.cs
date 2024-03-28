using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Requests.Justifications
{
    public class EmployeeGetRequestJustificationCriteria : BaseCriteria
    {
        public int? JustificationTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
