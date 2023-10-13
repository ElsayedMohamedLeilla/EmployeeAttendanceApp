using Dawem.Models.Response.Others;

namespace Dawem.Contract.BusinessLogic.Others
{
    public interface IUserScreenActionPermissionBL
    {
        GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions();
    }
}
