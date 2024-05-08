using Dawem.Enums.Generals;

namespace Dawem.Models.DTOs.Dawem.RealTime.Firebase
{
    public class SendNotificationsAndEmailsModel
    {
        public string ShortMessege { get; set; }
        public string FullMessage { get; set; }
        public List<int> UserIds { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public string NotificationDescription { get; set; }

    }
}
