using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Core.NotificationsStores;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface INotificationStoreBL
    {
        Task<bool> MarkAsRead(int notificationStoreId);
        Task<GetNotificationStoreResponseDTO> Get(GetNotificationStoreCriteria model);
        Task<GetNotificationStoreResponseDTO> GetNotificationsByUserId(int userId);
        public Task<bool> Enable(int GroupId);
        public Task<bool> Disable(DisableModelDTO model);
        public Task<bool> Delete(int GroupId);
        Task<int> GetUnreadNotificationCount(int userId);


    }
}
