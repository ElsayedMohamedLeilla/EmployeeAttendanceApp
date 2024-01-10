using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Permissions.Permissions;
using Dawem.Models.Response.Permissions.Permissions;
using Dawem.Models.Response.Schedules.SchedulePlanLogs;

namespace Dawem.Contract.BusinessLogic.Permissions
{
    public interface IPermissionBL
    {
        Task<int> Create(CreatePermissionModel model);
        Task<bool> Update(UpdatePermissionModel model);
        Task<GetPermissionInfoResponseModel> GetInfo(int permissionId);
        Task<GetPermissionScreensResponse> GetPermissionScreens(GetPermissionScreensCriteria model);
        Task<GetPermissionByIdResponseModel> GetById(int permissionId);
        Task<GetPermissionsResponse> Get(GetPermissionsCriteria model);
        Task<bool> Delete(int permissionId);
        Task<GetPermissionsInformationsResponseDTO> GetPermissionsInformations();
        Task<bool> CheckUserPermission(CheckUserPermissionModel model);
        Task<GetUserPermissionsResponseModel> GetCurrentUserPermissions(GetCurrentUserPermissionsModel? model = null);
    }
}
