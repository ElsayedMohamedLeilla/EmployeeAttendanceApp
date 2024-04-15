using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Models.Response.AdminPanel.Subscriptions.Plans;

namespace Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions
{
    public interface ISubscriptionPaymentBL
    {
        Task<int> Create(CreateSubscriptionPaymentModel model);
        Task<bool> Update(UpdateSubscriptionPaymentModel model);
        Task<GetSubscriptionPaymentInfoResponseModel> GetInfo(int planId3);
        Task<GetSubscriptionPaymentByIdResponseModel> GetById(int planId);
        Task<GetSubscriptionPaymentsResponse> Get(GetSubscriptionPaymentsCriteria model);
        Task<bool> Delete(int planId);
        Task<bool> Enable(int planId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetSubscriptionPaymentsInformationsResponseDTO> GetSubscriptionPaymentsInformations();
    }
}
