using Dawem.Models.Dtos.Employees.Departments;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface ISubscriptionBLValidation
    {
        Task<bool> CreateValidation(CreateSubscriptionModel model);
        Task<bool> UpdateValidation(UpdateSubscriptionModel model);
    }
}
