using Dawem.Models.Dtos.Employees.AssignmentType;
using Dawem.Models.Response.Employees.AssignmentTypes;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IAssignmentTypeBL
    {
        Task<int> Create(CreateAssignmentTypeModel model);
        Task<bool> Update(UpdateAssignmentTypeModel model);
        Task<GetAssignmentTypeInfoResponseModel> GetInfo(int assignmentTypeId);
        Task<GetAssignmentTypeByIdResponseModel> GetById(int assignmentTypeId);
        Task<GetAssignmentTypesResponse> Get(GetAssignmentTypesCriteria model);
        Task<GetAssignmentTypesForDropDownResponse> GetForDropDown(GetAssignmentTypesCriteria model);
        Task<bool> Delete(int assignmentTypeId);
    }
}
