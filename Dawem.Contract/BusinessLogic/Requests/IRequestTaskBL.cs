using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Tasks;
using Dawem.Models.Response.Requests.Task;

namespace Dawem.Contract.BusinessLogic.Requests
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
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
    }
}
