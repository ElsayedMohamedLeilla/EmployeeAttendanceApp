using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.General;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Ohters
{
    [Table(nameof(UserScreenActionPermission) + DawemKeys.S)]
    public class UserScreenActionPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual MyUser? User { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual Group? Group { get; set; }
        public ApplicationScreenType ActionPlace { get; set; }
        public ApiMethod ActionType { get; set; }
    }
}
