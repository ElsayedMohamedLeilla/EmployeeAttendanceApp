using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Core.NotificationsStores;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface INotificationStoreBL
    {
        Task<bool> MarkAsRead(int notificationStoreId);
        Task<bool> MarkAsViewed();
        Task<GetNotificationStoreResponseDTO> Get(GetNotificationStoreCriteria criteria);
        Task<GetNotificationStoreResponseDTO> GetNotifications(GetNotificationStoreCriteria criteria);
        public Task<bool> Enable(int GroupId);
        public Task<bool> Disable(DisableModelDTO model);
        public Task<bool> Delete(int GroupId);
        Task<int> GetUnreadNotificationCount();
        Task<GetNotificationStoreResponseDTO> GetUnreadNotification(GetNotificationStoreCriteria criteria);
        Task<int> GetUnViewedNotificationCount();

    }
}
