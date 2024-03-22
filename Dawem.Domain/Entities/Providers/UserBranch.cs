using Dawem.Domain.Entities.UserManagement;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Providers
{
    [Table(nameof(UserBranch) + LeillaKeys.ES)]
    public class UserBranch : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual MyUser User { get; set; }

        public int BranchId { get; set; }
    }
}
