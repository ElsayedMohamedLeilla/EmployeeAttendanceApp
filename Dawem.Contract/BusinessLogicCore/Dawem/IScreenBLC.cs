using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Models.Response.Dawem.Others;

namespace Dawem.Contract.BusinessLogicCore.Dawem
{
    public interface IScreenBLC
    {
        Task<GetAllScreensWithAvailableActionsResponse> GetAllScreensWithAvailableActions(GetScreensCriteria criteria);
    }
}
