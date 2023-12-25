using Dawem.Models.Dtos.Employees.TaskType;
using Dawem.Models.Response.Employees.TaskTypes;
using Dawem.Models.Response.Requests.Vacations;

namespace Dawem.Contract.BusinessLogic.Employees
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
