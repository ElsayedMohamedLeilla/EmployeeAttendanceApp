using Dawem.Models.Response.Others;

namespace SmartBusinessERP.BusinessLogic.Others.Contract
{
    public interface IUserScreenActionPermissionBL
    {
        GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions();
    }
}
