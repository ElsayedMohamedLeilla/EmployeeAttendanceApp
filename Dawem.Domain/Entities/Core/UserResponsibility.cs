using Dawem.Domain.Entities.UserManagement;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(LeillaKeys.UserResponsibilities)]
    public class UserResponsibility : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public MyUser User { get; set; }
        public int ResponsibilityId { get; set; }
        [ForeignKey(nameof(ResponsibilityId))]
        public Responsibility Responsibility { get; set; }
    }
}
