using Dawem.Contract.BusinessLogic.Schedules.SchedulePlanLogs;
using Dawem.Models.Dtos.Schedules.SchedulePlanLogs;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Schedules
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class SchedulePlanLogController : BaseController
    {
        private readonly ISchedulePlanLogBL schedulePlanLogBL;


        public SchedulePlanLogController(ISchedulePlanLogBL _schedulePlanLogBL)
        {
            schedulePlanLogBL = _schedulePlanLogBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSchedulePlanLogCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }

            var schedulePlanLogResponse = await schedulePlanLogBL.Get(criteria);
            return Success(schedulePlanLogResponse.SchedulePlanLogs, schedulePlanLogResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int schedulePlanLogId)
        {
            if (schedulePlanLogId < 1)
            {
                return BadRequest();
            }

            return Success(await schedulePlanLogBL.GetInfo(schedulePlanLogId));
        }

    }
}