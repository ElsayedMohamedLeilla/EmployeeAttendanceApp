using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Core.NotificationsStores
{
    public class CreateNotificationDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public NotificationStatus Status { get; set; }
        public string IconUrl { get; set; }
        public NotificationPriority Priority { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int RecipientUserId { get; set; }
    }
}
