namespace Dawem.Models.Response.Dawem.Core.Notifications
{
    public class GetNotificationsResponseDTO
    {
        public List<NotificationForGridDTO> Notifications { get; set; }
        public int TotalCount { get; set; }
    }
}
