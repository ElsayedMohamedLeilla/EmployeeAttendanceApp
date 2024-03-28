using Dawem.Models.Dtos.Dawem.Requests;
using Dawem.Models.Response.Requests.Requests;

namespace Dawem.Contract.BusinessLogic.Dawem.Requests
{
    public interface IRequestBL
    {
        Task<GetRequestInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestsResponse> Get(GetRequestsCriteria model);
        Task<EmployeeGetRequestsResponse> EmployeeGet(EmployeeGetRequestsCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
        Task<GetRequestsInformationsResponseDTO> GetRequestsInformations();
    }
}
