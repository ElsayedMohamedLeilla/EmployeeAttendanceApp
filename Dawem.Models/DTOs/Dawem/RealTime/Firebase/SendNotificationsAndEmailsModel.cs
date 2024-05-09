using Dawem.Enums.Generals;

namespace Dawem.Models.DTOs.Dawem.RealTime.Firebase
{
    public class SendNotificationsAndEmailsModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<int> UserIds { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

    }
}
