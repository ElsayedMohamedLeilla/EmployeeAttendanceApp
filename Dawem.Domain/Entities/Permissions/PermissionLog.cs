using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Permissions
{
    [Table(nameof(PermissionLog) + LeillaKeys.S)]
    public class PermissionLog : BaseEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual MyUser User { get; set; }
        public ApplicationScreenCode ScreenCode { get; set; }
        public ApplicationAction ActionCode { get; set; }
    }
}
