using Dawem.Models.Criteria.Core;
using Dawem.Models.Response.Dawem.Core.Notifications;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface INotificationBL
    {
        Task<bool> MarkAsRead(int notificationId);
        Task<bool> MarkAsViewed();
        Task<GetNotificationsResponseDTO> Get(GetNotificationCriteria criteria);
        Task<int> GetUnreadNotificationCount();
        Task<int> GetUnViewedNotificationCount();


    }
}
