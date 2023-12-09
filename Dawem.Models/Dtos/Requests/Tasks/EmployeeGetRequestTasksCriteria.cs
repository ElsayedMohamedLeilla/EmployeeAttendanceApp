using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Requests.Tasks
{
    public class EmployeeGetRequestTasksCriteria : BaseCriteria
    {
        public int? TaskTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
