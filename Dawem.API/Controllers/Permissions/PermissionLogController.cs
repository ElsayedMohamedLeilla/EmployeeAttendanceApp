using Dawem.Contract.BusinessLogic.Permissions;
using Dawem.Models.Criteria.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Others
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize(Roles = LeillaKeys.RoleFULLACCESS)]
    public class PermissionLogController : BaseController
    {
        private readonly IPermissionLogBL actionLogBL;

        public PermissionLogController(IPermissionLogBL _actionLogBL)
        {
            actionLogBL = _actionLogBL;
        }
        [HttpPost]
        public async Task<ActionResult> Get(GetPermissionLogsCriteria criteria)
        {
            var actionLogsRes = await actionLogBL.Get(criteria);
            return Success(actionLogsRes.ActionLogs, actionLogsRes.TotalCount);
        }
        [HttpPost]
        public async Task<ActionResult> GetInfo(GetActionLogInfoCriteria criteria)
        {
            return Success(await actionLogBL.GetInfo(criteria));
        }
    }
}