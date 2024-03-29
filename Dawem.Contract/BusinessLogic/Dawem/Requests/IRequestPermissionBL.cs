using Dawem.Models.Requests;
using Dawem.Models.Requests.Permissions;
using Dawem.Models.Response.Dawem.Requests.Permissions;

namespace Dawem.Contract.BusinessLogic.Dawem.Requests
{
    public interface IRequestPermissionBL
    {
        Task<int> Create(CreateRequestPermissionModelDTO model);
        Task<bool> Update(UpdateRequestPermissionModelDTO model);
        Task<GetRequestPermissionInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestPermissionByIdResponseModel> GetById(int requestId);
        Task<GetRequestPermissionsResponse> Get(GetRequestPermissionsCriteria model);
        Task<EmployeeGetRequestPermissionsResponseDTO> EmployeeGet(EmployeeGetRequestPermissionsCriteria model);
        Task<GetRequestPermissionsForDropDownResponse> GetForDropDown(GetRequestPermissionsCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
        Task<GetPermissionsInformationsResponseDTO> GetPermissionsInformations();
    }
}
