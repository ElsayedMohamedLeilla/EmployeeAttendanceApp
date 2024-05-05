using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Permissions
{
    [Table(nameof(PermissionScreenAction) + LeillaKeys.S)]
    public class PermissionScreenAction : BaseEntity
    {
        public int PermissionScreenId { get; set; }
        [ForeignKey(nameof(PermissionScreenId))]
        public virtual PermissionScreen PermissionScreen { get; set; }
        public DawemAdminApplicationAction ActionCode { get; set; }
    }
}
