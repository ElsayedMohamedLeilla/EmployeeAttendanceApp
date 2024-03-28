using Dawem.Models.Dtos.Dawem.Permissions.PermissionLogs;
using Dawem.Models.Response.Permissions.PermissionLogs;

namespace Dawem.Contract.BusinessLogic.Dawem.Permissions
{
    public interface IPermissionLogBL
    {
        Task<GetPermissionLogInfoResponseModel> GetInfo(int permissionLogId);
        Task<GetPermissionLogsResponse> Get(GetPermissionLogsCriteria model);
    }
}
