using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Enums.Permissions;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Screens
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class ScreenController : DawemControllerBase
    {
        private readonly IScreenBLC screenBLC;

        public ScreenController(IScreenBLC _screenBLC)
        {
            screenBLC = _screenBLC;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllScreensWithAvailableActions()
        {
            return Success(await screenBLC.GetAllScreensWithAvailableActions(new GetScreensCriteria { IsActive = true, ScreensForType = ScreensForType.AllScreens }));
        }
    }
}