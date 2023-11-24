using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(UserScreenActionPermission) + LeillaKeys.S)]
    public class UserScreenActionPermission : BaseEntity
    {
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual MyUser User { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }
        public ApplicationScreenType ActionPlace { get; set; }
        public ApiMethod ActionType { get; set; }
    }
}
