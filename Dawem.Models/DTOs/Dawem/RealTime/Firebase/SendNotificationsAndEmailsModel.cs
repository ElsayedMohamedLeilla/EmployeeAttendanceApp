using Dawem.Enums.Generals;
using Dawem.Models.Criteria.Core;

namespace Dawem.Models.DTOs.Dawem.RealTime.Firebase
{
    public class SendNotificationsAndEmailsModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        //public List<int> UserIds { get; set; }
        public List<NotificationUserModel> NotificationUsers { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

    }
}
