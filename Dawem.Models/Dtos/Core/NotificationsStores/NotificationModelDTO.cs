using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Core.NotificationsStores
{
    public class NotificationModelDTO
    {
        public int UnreadNotificationCount { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
