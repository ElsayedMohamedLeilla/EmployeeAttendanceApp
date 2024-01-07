using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;
using Dawem.Models.Response.Employees.AssignmentTypes;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface INonComplianceActionBL
    {
        Task<int> Create(CreateNonComplianceActionModel model);
        Task<bool> Update(UpdateNonComplianceActionModel model);
        Task<GetNonComplianceActionInfoResponseModel> GetInfo(int nonComplianceActionId);
        Task<GetNonComplianceActionByIdResponseModel> GetById(int nonComplianceActionId);
        Task<GetNonComplianceActionsResponse> Get(GetNonComplianceActionsCriteria model);
        Task<GetNonComplianceActionsForDropDownResponse> GetForDropDown(GetNonComplianceActionsCriteria model);
        Task<bool> Delete(int nonComplianceActionId);
        Task<GetNonComplianceActionsInformationsResponseDTO> GetNonComplianceActionsInformations();
    }
}
