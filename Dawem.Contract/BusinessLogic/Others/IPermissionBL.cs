using Dawem.Models.Criteria.Others;
using Dawem.Models.Response.Others;

namespace Dawem.Contract.BusinessLogic.Others
{
    public interface IPermissionBL
    {
        GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions();
        Task<bool> CheckUserPermission(CheckUserPermissionModel model);
    }
}
