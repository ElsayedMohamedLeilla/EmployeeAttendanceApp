using Dawem.API.Controllers;
using Dawem.Contract.BusinessLogic.Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBusinessERP.Domain.Entities.Lookups;
using SmartBusinessERP.Models.Dtos.Lookups;

namespace Dawem.API.Controllers.Provider
{
    [Route(DawemKeys.ApiCcontrollerAction)]
    [ApiController]
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

            var result = screenBL.DeleteScreen(Id);
            return Ok(result);
        }



    }
}