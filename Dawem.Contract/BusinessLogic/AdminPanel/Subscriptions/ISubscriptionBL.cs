using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Models.Response.AdminPanel.Subscriptions;
using Dawem.Models.Response.AdminPanel.Subscriptions.Plans;

namespace Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions
{
    public interface ISubscriptionBL
    {
        Task<int> Create(CreateSubscriptionModel model);
        Task<bool> Update(UpdateSubscriptionModel model);
        Task<GetSubscriptionInfoResponseModel> GetInfo(int subscriptionId);
        Task<GetSubscriptionByIdResponseModel> GetById(int subscriptionId);
        Task<GetSubscriptionsResponse> Get(GetSubscriptionsCriteria model);
        Task<GetSubscriptionsForDropDownResponse> GetForDropDown(GetSubscriptionsCriteria model);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int subscriptionId);
        Task<bool> Approve(ApproveSubscriptionModel model);
        Task<bool> Delete(int subscriptionId);
        Task<GetSubscriptionsInformationsResponseDTO> GetSubscriptionsInformations();
        Task HandleSubscriptions();
    }
}
