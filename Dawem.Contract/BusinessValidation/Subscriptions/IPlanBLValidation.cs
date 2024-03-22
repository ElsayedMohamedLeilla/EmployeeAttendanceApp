using Dawem.Models.Dtos.Subscriptions.Plans;

namespace Dawem.Contract.BusinessValidation.Subscriptions
{
    public interface IPlanBLValidation
    {
        Task<bool> CreateValidation(CreatePlanModel model);
        Task<bool> UpdateValidation(UpdatePlanModel model);
    }
}
