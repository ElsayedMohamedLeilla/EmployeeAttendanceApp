using Dawem.Enums.Generals;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Core.NotificationsStores;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface INotificationStoreBL
    {
        Task<bool> MarkAsRead(int notificationStoreId);
        Task<GetNotificationStoreResponseDTO> Get(GetNotificationStoreCriteria criteria);
        Task<GetNotificationStoreResponseDTO> GetNotificationsByUserId(GetNotificationStoreCriteria criteria);
        public Task<bool> Enable(int GroupId);
        public Task<bool> Disable(DisableModelDTO model);
        public Task<bool> Delete(int GroupId);
        Task<int> GetUnreadNotificationCountByUserId();
        Task<GetNotificationStoreResponseDTO> GetUnreadNotificationByUserId(GetNotificationStoreCriteria criteria);
        Task<bool> SendNotificationAndEmail(NotificationType type, int groupUserId, string EmployeeName, string employeeEmail);

    }
}
