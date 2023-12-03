using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Employees.JobTitle;
using Dawem.Models.Dtos.Requests.Task;
using Dawem.Models.Response.Employees.TaskTypes;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IRequestTaskBL
    {
        Task<int> Create(CreateRequestTaskModelDTO model);
        Task<bool> Update(UpdateRequestTaskModelDTO model);
        Task<GetRequestTaskInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestTaskByIdResponseModel> GetById(int requestId);
        Task<GetRequestTasksResponse> Get(GetRequestTasksCriteria model);
        Task<GetRequestTasksForDropDownResponse> GetForDropDown(GetRequestTasksCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Refuse(RefuseModelDTO refuseModelDTO);
        Task<bool> Delete(int requestId);
    }
}
