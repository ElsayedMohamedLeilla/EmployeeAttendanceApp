using Dawem.Enums.Generals;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;

namespace Dawem.Contract.RealTime.Firebase
{
    public interface INotificationServiceByFireBaseAdmin
    {
        Task<ResponseModel> Send_Notification_Email(List<int> UserIds, NotificationType notificationType, NotificationStatus type);
    }
}
