using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Core.NotificationsStores
{
    public class NotificationStoreForGridDTO
    {
        public int Id { get; set; }
        public string ShortMessege { get; set; }
        public string FullMessege { get; set; }
        public bool IsRead { get; set; }
        public NotificationStatus Status { get; set; }
        public string IconUrl { get; set; }
        public Priority Priority { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
