using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Permissions
{
    [Table(nameof(PermissionScreen) + LeillaKeys.S)]
    public class PermissionScreen : BaseEntity
    {
        public int PermissionId { get; set; }
        [ForeignKey(nameof(PermissionId))]
        public virtual Permission Permission { get; set; }
        public ApplicationScreenCode ScreenCode { get; set; }
        public List<PermissionScreenAction> PermissionScreenActions { get; set; }
    }
}
