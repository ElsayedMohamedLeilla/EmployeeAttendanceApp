using Dawem.API.MiddleWares.Helpers;
using Dawem.Contract.BusinessLogic.Others;
using Dawem.Models.Context;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Permissions
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class PermissionController : BaseController
    {
        private readonly IPermissionBL permissionBL;
        private readonly RequestInfo requestInfo;

        public PermissionController(RequestInfo _requestInfo)
        {
            requestInfo = _requestInfo;
        }
        [HttpGet]
        public ActionResult GetAllScreensWithAvailableActions()
        {
            var response = ControllerActionHelper.GetAllScreensWithAvailableActions(requestInfo);
            return Success(response, response.Screens.Count);
        }
    }
}