using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Core.NotificationsStores
{
    public class CreateNotificationStoreDTO
    {
        public string ShortMessege { get; set; }
        public string FullMessege { get; set; }
        public bool IsRead { get; set; }
        public NotificationStatus Status { get; set; }
        public string IconUrl { get; set; }
        public Priority Priority { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int RecipientUserId { get; set; }
    }
}
