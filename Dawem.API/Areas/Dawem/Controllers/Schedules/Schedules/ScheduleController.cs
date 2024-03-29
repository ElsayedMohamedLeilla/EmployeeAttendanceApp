using Dawem.Contract.BusinessLogic.Dawem.Schedules.Schedules;
using Dawem.Models.Dtos.Dawem.Schedules.Schedules;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Schedules.Schedules
{
    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleBL scheduleBL;


        public ScheduleController(IScheduleBL _scheduleBL)
        {
            scheduleBL = _scheduleBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create(CreateScheduleModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await scheduleBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateScheduleSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update(UpdateScheduleModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await scheduleBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateScheduleSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSchedulesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var schedulesresponse = await scheduleBL.Get(criteria);

            return Success(schedulesresponse.Schedules, schedulesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetSchedulesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var schedulesresponse = await scheduleBL.GetForDropDown(criteria);

            return Success(schedulesresponse.Schedules, schedulesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int scheduleId)
        {
            if (scheduleId < 1)
            {
                return BadRequest();
            }
            return Success(await scheduleBL.GetInfo(scheduleId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int scheduleId)
        {
            if (scheduleId < 1)
            {
                return BadRequest();
            }
            return Success(await scheduleBL.GetById(scheduleId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int scheduleId)
        {
            if (scheduleId < 1)
            {
                return BadRequest();
            }
            return Success(await scheduleBL.Delete(scheduleId));
        }
        [HttpGet]
        public async Task<ActionResult> GetSchedulesInformations()
        {
            return Success(await scheduleBL.GetSchedulesInformations());
        }

    }
}