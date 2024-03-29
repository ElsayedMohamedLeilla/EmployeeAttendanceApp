using Dawem.Models.Requests;
using Dawem.Models.Requests.Tasks;
using Dawem.Models.Response.Dawem.Requests.Tasks;

namespace Dawem.Contract.BusinessLogic.Dawem.Requests
{
    public interface IRequestTaskBL
    {
        Task<int> Create(CreateRequestTaskModelDTO model);
        Task<bool> Update(UpdateRequestTaskModelDTO model);
        Task<GetRequestTaskInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestTaskByIdResponseModel> GetById(int requestId);
        Task<GetRequestTasksResponse> Get(GetRequestTasksCriteria model);
        Task<List<EmployeeGetRequestTasksResponseModel>> EmployeeGet(EmployeeGetRequestTasksCriteria model);
        Task<GetRequestTasksForDropDownResponse> GetForDropDown(GetRequestTasksCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
        Task<GetTasksInformationsResponseDTO> GetTasksInformations();
    }
}
