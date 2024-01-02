using Dawem.Models.Dtos.Permissions.PermissionLogs;
using Dawem.Models.Response.Permissions.PermissionLogs;

namespace Dawem.Contract.BusinessLogic.Permissions
{
    public interface IPermissionLogBL
    {
        Task<GetPermissionLogInfoResponseModel> GetInfo(int permissionLogId);
        Task<GetPermissionLogsResponse> Get(GetPermissionLogsCriteria model);
    }
}
