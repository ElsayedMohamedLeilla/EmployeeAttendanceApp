using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Requests.Assignments
{
    public class Employee2GetRequestAssignmentsCriteria : BaseCriteria
    {
        public int? AssignmentTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
