using Dawem.Models.Requests;
using Dawem.Models.Requests.Justifications;
using Dawem.Models.Response.Dawem.Requests.Permissions;

namespace Dawem.Contract.BusinessLogic.Dawem.Requests
{
    public interface IRequestOvertimeBL
    {
        Task<int> Create(CreateRequestOvertimeDTO model);
        Task<bool> Update(UpdateRequestOvertimeDTO model);
        Task<GetRequestOvertimeInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestOvertimeByIdResponseModel> GetById(int requestId);
        Task<GetRequestOvertimesResponse> Get(GetRequestOvertimeCriteria model);
        Task<EmployeeGetRequestOvertimesResponseDTO> EmployeeGet(EmployeeGetRequestOvertimeCriteria model);
        Task<GetRequestOvertimesForDropDownResponse> GetForDropDown(GetRequestOvertimeCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
        Task<GetOvertimesInformationsResponseDTO> GetOvertimesInformations();
    }
}
