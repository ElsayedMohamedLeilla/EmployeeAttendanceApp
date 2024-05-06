using Dawem.Enums.Generals;

namespace Dawem.Models.Criteria.Core
{
    public class HandleNotificationModel
    {
        public HandleNotificationModel()
        {
            UserIds = new List<int>();
            EmployeeIds = new List<int>();
        }
        public List<int> UserIds { get; set; }
        public List<int> EmployeeIds { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public Priority Priority { get; set; }
    }
}
