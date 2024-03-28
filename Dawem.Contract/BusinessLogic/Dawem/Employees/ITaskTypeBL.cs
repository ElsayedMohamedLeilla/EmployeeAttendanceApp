using Dawem.Models.Dtos.Dawem.Employees.TaskTypes;
using Dawem.Models.Response.Dawem.Employees.TaskTypes;

namespace Dawem.Contract.BusinessLogic.Dawem.Employees
{
    public interface ITaskTypeBL
    {
        Task<int> Create(CreateTaskTypeModel model);
        Task<bool> Update(UpdateTaskTypeModel model);
        Task<GetTaskTypeInfoResponseModel> GetInfo(int taskTypeId);
        Task<GetTaskTypeByIdResponseModel> GetById(int taskTypeId);
        Task<GetTaskTypesResponse> Get(GetTaskTypesCriteria model);
        Task<GetTaskTypesForDropDownResponse> GetForDropDown(GetTaskTypesCriteria model);
        Task<bool> Delete(int taskTypeId);
        Task<GetTaskTypesInformationsResponseDTO> GetTaskTypesInformations();
    }
}
