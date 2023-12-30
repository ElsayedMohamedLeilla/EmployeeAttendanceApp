using Dawem.Contract.BusinessLogic.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Others
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize(Roles = LeillaKeys.RoleFULLACCESS)]
    public class PermissionController : BaseController
    {
        private readonly IPermissionBL permissionBL;

        public PermissionController(IPermissionBL _permissionBL)
        {
            permissionBL = _permissionBL;
        }
        [HttpPost]
        public ActionResult GetAllScreensWithAvailableActions()
        {
            return Ok(permissionBL.GetAllScreensWithAvailableActions());
        }

    }
}
