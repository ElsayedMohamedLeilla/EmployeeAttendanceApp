using Dawem.Models.Dtos.Employees.AssignmentTypes;
using Dawem.Models.Response.Employees.AssignmentTypes;

namespace Dawem.Contract.BusinessLogic.Permissions
{
    public interface IPermissionLogBL
    {
        Task<GetPermissionLogInfoResponseModel> GetInfo(int permissionLogId);
        Task<GetPermissionLogsResponse> Get(GetPermissionLogsCriteria model);
    }
}
