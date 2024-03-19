using Dawem.Domain.Entities;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.RealTime.Firebase
{
    [Table(nameof(NotificationUserFCMToken) + LeillaKeys.S)]
    public class NotificationUserFCMToken : BaseEntity
    {
        #region Foregn Keys
        public int NotificationUserId { get; set; }
        [ForeignKey(nameof(NotificationUserId))]
        public NotificationUser NotificationUser { get; set; }
        #endregion
        public string FCMToken { get; set; }
        public ApplicationType DeviceType { get; set; }
        public DateTime LastLogInDate { get; set; }
    }
}
