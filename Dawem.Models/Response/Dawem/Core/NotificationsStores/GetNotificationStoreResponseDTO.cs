namespace Dawem.Models.Response.Dawem.Core.NotificationsStores
{
    public class GetNotificationStoreResponseDTO
    {
        public List<NotificationStoreForGridDTO> NotificationStores { get; set; }
        public int TotalCount { get; set; }
    }
}
