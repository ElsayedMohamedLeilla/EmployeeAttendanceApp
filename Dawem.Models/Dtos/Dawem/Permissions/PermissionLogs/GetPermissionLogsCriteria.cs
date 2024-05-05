using Dawem.Enums.Permissions;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Permissions.PermissionLogs
{
    public class GetPermissionLogsCriteria : BaseCriteria
    {
        public int? UserId { get; set; }
        public int? ScreenCode { get; set; }
        public DawemAdminApplicationAction? ActionCode { get; set; }
    }
}
