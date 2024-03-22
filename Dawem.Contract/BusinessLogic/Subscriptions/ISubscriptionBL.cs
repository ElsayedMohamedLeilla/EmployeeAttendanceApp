using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Employees.Departments;

namespace Dawem.Contract.BusinessLogic.Subscriptions
{
    public interface ISubscriptionBL
    {
        Task<int> Create(CreateSubscriptionModel model);
        Task<bool> Update(UpdateSubscriptionModel model);
        Task<GetSubscriptionInfoResponseModel> GetInfo(int subscriptionId);
        Task<GetSubscriptionByIdResponseModel> GetById(int subscriptionId);
        Task<GetSubscriptionsResponse> Get(GetSubscriptionsCriteria model);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int subscriptionId);
        Task<bool> Delete(int subscriptionId);
        Task<GetSubscriptionsInformationsResponseDTO> GetSubscriptionsInformations();
        Task HandleSubscriptions();
    }
}
