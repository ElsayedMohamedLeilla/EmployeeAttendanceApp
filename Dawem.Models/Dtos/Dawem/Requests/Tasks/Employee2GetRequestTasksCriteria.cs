using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Requests.Tasks
{
    public class Employee2GetRequestTasksCriteria : BaseCriteria
    {
        public int? TaskTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
