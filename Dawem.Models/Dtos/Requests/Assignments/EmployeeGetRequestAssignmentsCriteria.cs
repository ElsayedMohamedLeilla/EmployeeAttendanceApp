using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Requests.Assignments
{
    public class EmployeeGetRequestAssignmentsCriteria : BaseCriteria
    {
        public int? AssignmentTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
