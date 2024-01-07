using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IFingerprintEnforcementBLValidation
    {
        Task<bool> CreateValidation(CreateFingerprintEnforcementModel model);
        Task<bool> UpdateValidation(UpdateFingerprintEnforcementModel model);
    }
}
