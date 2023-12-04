using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Tasks;
using Dawem.Models.Response.Requests.Task;

namespace Dawem.Contract.BusinessLogic.Requests
{
    public interface IRequestAssignmentBL
    {
        Task<int> Create(CreateRequestAssignmentModelDTO model);
        Task<bool> Update(UpdateRequestAssignmentModelDTO model);
        Task<GetRequestAssignmentInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestAssignmentByIdResponseModel> GetById(int requestId);
        Task<GetRequestAssignmentsResponse> Get(GetRequestAssignmentsCriteria model);
        Task<GetRequestAssignmentsForDropDownResponse> GetForDropDown(GetRequestAssignmentsCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
    }
}
