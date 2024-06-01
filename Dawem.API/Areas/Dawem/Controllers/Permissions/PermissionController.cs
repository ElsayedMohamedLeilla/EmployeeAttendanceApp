using Dawem.API.Helpers;
using Dawem.Contract.BusinessLogic.Dawem.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Permissions
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class PermissionController : DawemControllerBase
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
            #region Set All Screens Available Actions

            APIHelper.AllScreensWithAvailableActions ??= ControllerActionHelper.GetAllScreensWithAvailableActions(requestInfo);

            #endregion

            var result = await permissionBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreatePermissionSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdatePermissionModel model)
        {
            #region Set All Screens Available Actions

            APIHelper.AllScreensWithAvailableActions ??= ControllerActionHelper.GetAllScreensWithAvailableActions(requestInfo);

            #endregion

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
        public async Task<ActionResult> GetPermissionScreens([FromQuery] GetPermissionScreensCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            return Success(await permissionBL.GetPermissionScreens(criteria));
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
        [HttpGet]
        public async Task<ActionResult> CheckAndGetPermission([FromQuery] CheckAndGetPermissionModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            return Success(await permissionBL.CheckAndGetPermission(model));
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
        public ActionResult GetAllScreensWithAvailableActions([FromQuery] bool IsForMenu)
        {
            var response = ControllerActionHelper.GetAllScreensWithAvailableActions(requestInfo, IsForMenu);
            return Success(response, response.Screens.Count);
        }
        [HttpGet]
        public async Task<ActionResult> GetCurrentUserPermissions()
        {
            var response = await permissionBL.GetCurrentUserPermissions();
            var count = response?.UserPermissions?.Count ?? 0;
            return Success(response, count);
        }
    }
}