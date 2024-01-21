using Dawem.Models.Firebase;

namespace Dawem.Contract.Firebase
{
    public interface INotificationServiceByFireBaseAdmin
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
    }
}
