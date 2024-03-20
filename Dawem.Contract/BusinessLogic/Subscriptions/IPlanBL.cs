using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Employees.Departments;

namespace Dawem.Contract.BusinessLogic.Employees.Plan
{
    public interface IPlanBL
    {
        Task<int> Create(CreatePlanModel model);
        Task<bool> Update(UpdatePlanModel model);
        Task<GetPlanInfoResponseModel> GetInfo(int planId3);
        Task<GetPlanByIdResponseModel> GetById(int planId);
        Task<GetPlansResponse> Get(GetPlansCriteria model);
        Task<GetPlansForDropDownResponse> GetForDropDown(GetPlansCriteria model);
        Task<bool> Delete(int planId);
        Task<bool> Enable(int planId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetPlansInformationsResponseDTO> GetPlansInformations();
    }
}
