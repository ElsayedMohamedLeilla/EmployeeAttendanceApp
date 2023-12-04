using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Requests.Tasks
{
    public class GetRequestAssignmentsCriteria : BaseCriteria
    {
        public int? AssignmentTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
