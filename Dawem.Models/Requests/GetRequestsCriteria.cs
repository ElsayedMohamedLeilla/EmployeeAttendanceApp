using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Requests
{
    public class GetRequestsCriteria : BaseCriteria
    {
        public RequestType? Type { get; set; }
        public int? EmployeeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
