using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Core.NotificationsStores
{
    public class NotificationDataModel
    {
        public int UnViewdNotificationCount { get; set; }
        public int UnReadNotificationCount { get; set; }
        public NotificationType NotificationType { get; set; }
        public string NotificationIcon { get; set; }
        public Priority Priority { get; set; }

    }
}
