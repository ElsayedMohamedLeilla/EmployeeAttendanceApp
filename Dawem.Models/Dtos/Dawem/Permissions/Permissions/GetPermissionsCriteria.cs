using Dawem.Enums.Permissions;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Permissions.Permissions
{
    public class GetPermissionsCriteria : BaseCriteria
    {
        public ApplicationScreenCode? ScreenCode { get; set; }
        public ApplicationAction? ActionCode { get; set; }
    }
}
