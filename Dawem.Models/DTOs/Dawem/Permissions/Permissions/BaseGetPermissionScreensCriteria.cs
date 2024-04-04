using Dawem.Enums.Permissions;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Permissions.Permissions
{
    public class BaseGetPermissionScreensCriteria : BaseCriteria
    {
        public int PermissionId { get; set; }
        public ApplicationAction? ActionCode { get; set; }
    }
}
