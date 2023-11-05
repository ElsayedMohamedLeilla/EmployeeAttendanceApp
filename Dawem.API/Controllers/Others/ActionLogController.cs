using Dawem.Contract.BusinessLogic.Others;
using Dawem.Models.Criteria.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Others
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize(Roles = LeillaKeys.FullAccess)]
    public class ActionLogController : BaseController
    {
        private readonly IActionLogBL actionLogBL;

        public ActionLogController(IActionLogBL _actionLogBL)
        {
            actionLogBL = _actionLogBL;
        }
        [HttpPost]
        public async Task<ActionResult> Get(GetActionLogsCriteria criteria)
        {
            var actionLogsRes = await actionLogBL.Get(criteria);
            return Success(actionLogsRes.ActionLogs, actionLogsRes.TotalCount);
        }
        [HttpPost]
        public async Task<ActionResult> GetInfo(GetActionLogInfoCriteria criteria)
        {
            return Success(await actionLogBL.GetInfo(criteria));
        }
        [HttpPost]
        public async Task<ActionResult> GetById([FromBody] int id)
        {
            return Success(await actionLogBL.GetById(id));
        }
    }
}