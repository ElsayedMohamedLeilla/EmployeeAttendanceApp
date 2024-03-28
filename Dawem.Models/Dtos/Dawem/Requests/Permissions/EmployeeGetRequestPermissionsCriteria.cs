using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Requests.Permissions
{
    public class EmployeeGetRequestPermissionsCriteria : BaseCriteria
    {
        public int? PermissionTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
