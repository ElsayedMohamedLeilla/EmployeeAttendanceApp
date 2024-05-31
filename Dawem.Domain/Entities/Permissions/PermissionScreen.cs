using Dawem.Domain.Entities.Others;
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
        public int ScreenId { get; set; }
        [ForeignKey(nameof(ScreenId))]
        public virtual Screen Screen { get; set; }
        //public int ScreenCode { get; set; } // ApplicationScreenCode or AdminPanelApplicationScreenCode
        public List<PermissionScreenAction> PermissionScreenActions { get; set; }
    }
}
