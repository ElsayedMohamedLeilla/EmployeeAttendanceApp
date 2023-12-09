using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Requests
{
    public class EmployeeGetRequestsCriteria : BaseCriteria
    {
        public RequestType? Type { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
