using Dawem.Enums.Permissions;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Permissions.Permissions
{
    public class BaseGetPermissionScreensCriteria : BaseCriteria
    {
        public int PermissionId { get; set; }
        public DawemAdminApplicationAction? ActionCode { get; set; }
    }
}
