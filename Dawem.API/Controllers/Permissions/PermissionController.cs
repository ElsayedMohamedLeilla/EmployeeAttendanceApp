using Dawem.API.MiddleWares.Helpers;
using Dawem.Contract.BusinessLogic.Permissions;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Permissions.Permissions;
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

        public PermissionController(IPermissionBL _permissionBL, RequestInfo _requestInfo)
        {
            permissionBL = _permissionBL;
            requestInfo = _requestInfo;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePermissionModel model)
        {
            var result = await permissionBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreatePermissionSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdatePermissionModel model)
        {

            var result = await permissionBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdatePermissionSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetPermissionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var response = await permissionBL.Get(criteria);

            return Success(response.Permissions, response.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int permissionId)
        {
            if (permissionId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionBL.GetInfo(permissionId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int permissionId)
        {
            if (permissionId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionBL.GetById(permissionId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int permissionId)
        {
            if (permissionId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionBL.Delete(permissionId));
        }
        [HttpGet]
        public async Task<ActionResult> GetPermissionsInformations()
        {
            return Success(await permissionBL.GetPermissionsInformations());
        }
        [HttpGet]
        public ActionResult GetAllScreensWithAvailableActions()
        {
            var response = ControllerActionHelper.GetAllScreensWithAvailableActions(requestInfo);
            return Success(response, response.Screens.Count);
        }
    }
}