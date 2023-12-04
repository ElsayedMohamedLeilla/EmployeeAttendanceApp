using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Tasks;
using Dawem.Models.Response.Requests.Task;

namespace Dawem.Contract.BusinessLogic.Requests
{
    public interface IRequestPermissionBL
    {
        Task<int> Create(CreateRequestPermissionModelDTO model);
        Task<bool> Update(UpdateRequestPermissionModelDTO model);
        Task<GetRequestPermissionInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestPermissionByIdResponseModel> GetById(int requestId);
        Task<GetRequestPermissionsResponse> Get(GetRequestPermissionsCriteria model);
        Task<GetRequestPermissionsForDropDownResponse> GetForDropDown(GetRequestPermissionsCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
    }
}
