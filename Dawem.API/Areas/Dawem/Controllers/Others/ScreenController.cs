using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Others
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
            return Success(await screenBLC.GetAllScreensWithAvailableActions());
        }
    }
}