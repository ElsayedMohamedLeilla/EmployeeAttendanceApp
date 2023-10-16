using Dawem.Domain.Entities.UserManagement;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(UserGroup) + DawemKeys.S)]
    public class UserGroup : BaseEntity
    {
        #region Foriegn Keys

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public MyUser User { get; set; }

        #endregion

    }
}
