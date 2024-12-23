using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Requests.Justifications
{
    public class EmployeeGetRequestOvertimeCriteria : BaseCriteria
    {
        public int? OvertimeTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
