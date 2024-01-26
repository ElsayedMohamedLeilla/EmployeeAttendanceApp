using Dawem.Domain.Entities;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.RealTime.Firebase
{
    [Table(nameof(NotificationUser) + LeillaKeys.S)]
    public class NotificationUser : BaseEntity
    {
        #region Foregn Keys
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public MyUser User { get; set; }
        #endregion
        public List<NotificationUserDeviceToken> NotificationUserDeviceTokens { get; set; }
    }
}
