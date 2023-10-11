using Dawem.Domain.Entities.UserManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(UserGroup) + "es")]
    public class UserGroup : BaseEntity
    {
        #region Foriegn Keys

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual Group? Group { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        #endregion

    }
}
