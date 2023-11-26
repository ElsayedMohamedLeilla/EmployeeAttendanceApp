using Dawem.Contract.BusinessLogic.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Others
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize(Roles = LeillaKeys.RoleFULLACCESS)]
    public class UserScreenActionPermissionController : BaseController
    {
        private readonly IUserScreenActionPermissionBL userScreenActionPermissionBL;

        public UserScreenActionPermissionController(IUserScreenActionPermissionBL _userScreenActionPermissionBL)
        {
            userScreenActionPermissionBL = _userScreenActionPermissionBL;
        }
        [HttpPost]
        public ActionResult GetAllScreensWithAvailableActions()
        {
            return Ok(userScreenActionPermissionBL.GetAllScreensWithAvailableActions());
        }

    }
}
