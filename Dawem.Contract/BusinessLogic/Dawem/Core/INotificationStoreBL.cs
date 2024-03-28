using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.Core.NotificationsStores;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface INotificationStoreBL
    {
        Task<bool> MarkAsRead(int notificationStoreId);
        Task<bool> MarkAsViewed();
        Task<GetNotificationStoreResponseDTO> Get(GetNotificationStoreCriteria criteria);
        Task<GetNotificationStoreResponseDTO> GetNotifications(GetNotificationStoreCriteria criteria);
        Task<bool> Enable(int GroupId);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Delete(int GroupId);
        Task<int> GetUnreadNotificationCount();
        Task<GetNotificationStoreResponseDTO> GetUnreadNotification(GetNotificationStoreCriteria criteria);
        Task<int> GetUnViewedNotificationCount();


    }
}
