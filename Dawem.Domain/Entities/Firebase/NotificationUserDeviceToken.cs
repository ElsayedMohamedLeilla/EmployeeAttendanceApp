using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Firebase
{
    [Table(nameof(NotificationUserDeviceToken) + LeillaKeys.S)]
    public class NotificationUserDeviceToken : BaseEntity
    {
        #region Foregn Keys
        public int NotificationUserId { get; set; }
        [ForeignKey(nameof(NotificationUserId))]
        public NotificationUser NotificationUser { get; set; }
        #endregion
        public string DeviceToken { get; set; }
        public ApplicationType DeviceType { get; set; }
        public DateTime LastLogInDate { get; set; }
    }
}
