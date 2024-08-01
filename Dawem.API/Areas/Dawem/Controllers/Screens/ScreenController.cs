using Dawem.BusinessLogic.AdminPanel.Subscriptions;
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
        private readonly IScreenBL screenBL;
        private readonly IScreenBLC screenBLC;

        public ScreenController(IScreenBL _screenBL,
            IScreenBLC _screenBLC)
        {
            screenBL = _screenBL;
            screenBLC = _screenBLC;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllScreensWithAvailableActions()
        {
            return Success(await screenBLC.GetAllScreensWithAvailableActions(new GetScreensCriteria { IsActive = true, ScreensForType = ScreensForType.AllScreens }));
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetScreensCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var screensResponse = await screenBL.GetForDropDown(criteria);

            return Success(screensResponse.Screens, screensResponse.TotalCount);
        }
    }
}