using Dawem.Contract.BusinessLogic.Attendances.SchedulePlanBackgroudJobLogs;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Attendances.SchedulePlans
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class SchedulePlanBackgroundJobLogController : BaseController
    {
        private readonly ISchedulePlanBackgroundJobLogBL schedulePlanBackgroundJobLogBL;


        public SchedulePlanBackgroundJobLogController(ISchedulePlanBackgroundJobLogBL _schedulePlanBackgroundJobLogBL)
        {
            schedulePlanBackgroundJobLogBL = _schedulePlanBackgroundJobLogBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSchedulePlanBackgroundJobLogsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }

            var schedulePlanBackgroundJobLogResponse = await schedulePlanBackgroundJobLogBL.Get(criteria);
            return Success(schedulePlanBackgroundJobLogResponse.SchedulePlanBackgroundJobLogs, schedulePlanBackgroundJobLogResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int schedulePlanBackgroundJobLogId)
        {
            if (schedulePlanBackgroundJobLogId < 1)
            {
                return BadRequest();
            }

            return Success(await schedulePlanBackgroundJobLogBL.GetInfo(schedulePlanBackgroundJobLogId));
        }

    }
}