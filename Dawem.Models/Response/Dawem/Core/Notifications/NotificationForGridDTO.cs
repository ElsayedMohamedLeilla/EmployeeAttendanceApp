using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Core.Notifications
{
    public class NotificationForGridDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public DawemAdminApplicationScreenCode ScreenCode { get; set; }
        public NotificationStatus Status { get; set; }
        public string IconUrl { get; set; }
        public NotificationPriority Priority { get; set; }
        public DateTime Date { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
