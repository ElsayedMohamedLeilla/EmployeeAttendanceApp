using Dawem.Enums.Generals;

namespace Dawem.Models.Criteria.Core
{
    public class HandleNotificationModel
    {
        public HandleNotificationModel()
        {
            UserIds = new List<int>();
            EmployeeIds = new List<int>();
            NotificationDescriptions = new List<NotificationDescriptionModel>();
            ActiveLanguages = new List<ActiveLanguageModel>();
            NotificationUsers = new List<NotificationUserModel>();
        }
        public int CompanyId { get; set; }
        public List<int> UserIds { get; set; }
        public List<NotificationUserModel> NotificationUsers { get; set; }
        public List<int> EmployeeIds { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public NotificationPriority Priority { get; set; }
        public int? HelperNumber { get; set; }
        public DateTime? HelperDate { get; set; }
        public List<NotificationDescriptionModel> NotificationDescriptions { get; set; }
        public List<ActiveLanguageModel> ActiveLanguages { get; set; }
    }
}
