using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Core.NotificationsStores
{
    public class NotificationParametersModel
    {
        public List<NotificationType> types { get; set; }
        public List<NotifyWay> notifyWays { get; set; }
        public List<int> departmentIds { get; set; }
        public List<int> groupIds { get; set; }
        public List<int> employeeIds { get; set; }
    }
}
