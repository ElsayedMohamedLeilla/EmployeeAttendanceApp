using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Models.Response.Dawem.Others;

namespace Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions
{
    public interface IScreenBLC
    {
        Task<GetAllScreensWithAvailableActionsResponse> GetAllScreensWithAvailableActions();
    }
}
