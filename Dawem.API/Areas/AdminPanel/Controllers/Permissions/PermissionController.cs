using Dawem.API.Areas.Dawem.Controllers;
using Dawem.API.Helpers;
using Dawem.Contract.BusinessLogic.Dawem.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Permissions
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, Authorize, AdminPanelAuthorize]
    public class PermissionController : AdminPanelControllerBase
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

            APIHelper.AdminPanelAllScreensWithAvailableActions ??= ControllerActionHelper.GetAllScreensWithAvailableActions(requestInfo);

            #endregion

            var result = await permissionBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreatePermissionSuccessfully);
        }
        [HttpPut]
        
        public async Task<ActionResult> Update(UpdatePermissionModel model)
        {
            #region Set All Screens Available Actions

            APIHelper.AdminPanelAllScreensWithAvailableActions ??= ControllerActionHelper.GetAllScreensWithAvailableActions(requestInfo);

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
        [HttpPut]
        
        public async Task<ActionResult> Enable(int responsibilityId)
        {
            if (responsibilityId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionBL.Enable(responsibilityId));
        }
        [HttpPut]
        
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await permissionBL.Disable(model));
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
        [HttpGet]
        
        public async Task<ActionResult> GetCurrentUserPermissions()
        {
            var response = await permissionBL.GetCurrentUserPermissions();
            var count = response?.UserPermissions?.Count ?? 0;
            return Success(response, count);
        }
    }
}