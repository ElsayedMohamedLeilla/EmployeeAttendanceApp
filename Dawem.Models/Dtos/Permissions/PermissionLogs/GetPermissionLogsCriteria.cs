using Dawem.Enums.Configration;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Employees.AssignmentTypes
{
    public class GetPermissionLogsCriteria : BaseCriteria
    {
        public int? UserId { get; set; }
        public ApplicationScreenCode? ScreenCode { get; set; }
        public ApplicationAction? ActionCode { get; set; }
    }
}
