using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface INonComplianceActionBLValidation
    {
        Task<bool> CreateValidation(CreateNonComplianceActionModel model);
        Task<bool> UpdateValidation(UpdateNonComplianceActionModel model);
    }
}
