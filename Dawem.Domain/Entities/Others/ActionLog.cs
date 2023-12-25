using System.ComponentModel.DataAnnotations.Schema;
using Dawem.Translations;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Domain.Entities.Providers;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(ActionLog) + LeillaKeys.S)]
    public class ActionLog : BaseEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual MyUser User { get; set; }
        public ApplicationScreenType ActionPlace { get; set; }
        public ApiMethod ActionType { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
