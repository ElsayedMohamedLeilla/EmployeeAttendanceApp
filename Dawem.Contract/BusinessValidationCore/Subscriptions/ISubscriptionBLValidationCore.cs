using Dawem.Models.Criteria.Others;
using Dawem.Models.Response.Employees.Departments;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface ISubscriptionBLValidationCore
    {
        Task<CheckCompanySubscriptionResponseModel> CheckCompanySubscription(CheckCompanySubscriptionModel model);
    }
}
