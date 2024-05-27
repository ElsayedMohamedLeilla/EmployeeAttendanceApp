using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Models.Response.Dawem.Permissions.Permissions;

namespace Dawem.Contract.BusinessLogic.Dawem.Permissions
{
    public interface IPermissionBL
    {
        Task<int> Create(CreatePermissionModel model);
        Task<bool> Update(UpdatePermissionModel model);
        Task<GetPermissionInfoResponseModel> GetInfo(int permissionId);
        Task<GetPermissionScreensResponse> GetPermissionScreens(GetPermissionScreensCriteria model);
        Task<GetPermissionByIdResponseModel> GetById(int permissionId);
        Task<GetPermissionByIdResponseModel> CheckAndGetPermission(CheckAndGetPermissionModel model);
        Task<GetPermissionsResponse> Get(GetPermissionsCriteria model);
        Task<bool> Delete(int permissionId);
        Task<GetPermissionsInformationsResponseDTO> GetPermissionsInformations();
        Task<bool> CheckUserPermission(CheckUserPermissionModel model);
        Task<bool> CheckScreenInPlan(CheckScreenInPlanModel model);
        Task<bool> Enable(int permissionId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetUserPermissionsResponseModel> GetCurrentUserPermissions(GetCurrentUserPermissionsModel? model = null);
    }
}
