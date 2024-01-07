using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;
using Dawem.Models.Response.Employees.AssignmentTypes;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IFingerprintEnforcementBL
    {
        Task<int> Create(CreateFingerprintEnforcementModel model);
        Task<bool> Update(UpdateFingerprintEnforcementModel model);
        Task<GetFingerprintEnforcementInfoResponseModel> GetInfo(int fingerprintEnforcementId);
        Task<GetFingerprintEnforcementByIdResponseModel> GetById(int fingerprintEnforcementId);
        Task<GetFingerprintEnforcementsResponse> Get(GetFingerprintEnforcementsCriteria model);
        Task<bool> Delete(int fingerprintEnforcementId);
        Task<GetFingerprintEnforcementsInformationsResponseDTO> GetFingerprintEnforcementsInformations();
    }
}
