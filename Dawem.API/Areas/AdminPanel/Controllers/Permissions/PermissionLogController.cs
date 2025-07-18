﻿using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.Dawem.Permissions;
using Dawem.Models.Dtos.Dawem.Permissions.PermissionLogs;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Permissions
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, AdminPanelAuthorize]
    public class PermissionLogController : AdminPanelControllerBase
    {
        private readonly IPermissionLogBL permissionLogBL;

        public PermissionLogController(IPermissionLogBL _permissionLogBL)
        {
            permissionLogBL = _permissionLogBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetPermissionLogsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var response = await permissionLogBL.Get(criteria);
            return Success(response.PermissionLogs, response.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int permissionLogId)
        {
            if (permissionLogId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionLogBL.GetInfo(permissionLogId));
        }
    }
}