using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Dawem.Translations;
using Dawem.Enums.General;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Domain.Entities.Provider;

namespace Dawem.Domain.Entities.Ohters
{
    [Table(nameof(ActionLog) + DawemKeys.S)]
    public class ActionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
