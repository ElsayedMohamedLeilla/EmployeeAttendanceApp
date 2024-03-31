using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Permissions
{
    [Table(nameof(Permission) + LeillaKeys.S)]
    public class Permission : BaseEntity
    {
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public ForRoleOrUser ForType { get; set; }
        public int? ResponsibilityId { get; set; }
        [ForeignKey(nameof(ResponsibilityId))]
        public virtual Responsibility Responsibility { get; set; }
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual MyUser User { get; set; }
        public int Code { get; set; }
        public List<PermissionScreen> PermissionScreens { get; set; }
    }
}
