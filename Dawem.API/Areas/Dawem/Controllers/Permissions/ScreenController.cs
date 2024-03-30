using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Dtos.Dawem.Lookups;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Permissions
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    public class ScreenController : BaseController
    {
        private readonly IScreenBL screenBL;

        public ScreenController(IScreenBL _screenBL)
        {
            screenBL = _screenBL;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<CreatedScreen>> Create(CreatedScreen screen)
        {

            if (screen == null)
            {
                return BadRequest();
            }

            var result = await screenBL.Create(screen);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Screen>> GetAllDescendantScreens([FromBody] int Id)
        {

            if (Id == default)
            {

                return BadRequest();

            }

            var result = screenBL.GetAllDescendantScreens(Id);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<IEnumerable<bool>> Delete([FromBody] int Id)
        {

            if (Id == default)
            {

                return BadRequest();

            }

            var result = screenBL.Delete(Id);
            return Ok(result);
        }



    }
}