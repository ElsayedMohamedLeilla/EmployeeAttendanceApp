using Dawem.Contract.BusinessLogic.Dawem.Schedules.SchedulePlans;
using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlans;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Schedules
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class SchedulePlanController : DawemControllerBase
    {
        private readonly ISchedulePlanBL schedulePlanBL;


        public SchedulePlanController(ISchedulePlanBL _schedulePlanBL)
        {
            schedulePlanBL = _schedulePlanBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create(CreateSchedulePlanModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await schedulePlanBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateSchedulePlanSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update(UpdateSchedulePlanModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await schedulePlanBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateSchedulePlanSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSchedulePlansCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var schedulePlansresponse = await schedulePlanBL.Get(criteria);

            return Success(schedulePlansresponse.SchedulePlans, schedulePlansresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetSchedulePlansCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var schedulePlansresponse = await schedulePlanBL.GetForDropDown(criteria);

            return Success(schedulePlansresponse.SchedulePlans, schedulePlansresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int schedulePlanId)
        {
            if (schedulePlanId < 1)
            {
                return BadRequest();
            }
            return Success(await schedulePlanBL.GetInfo(schedulePlanId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int schedulePlanId)
        {
            if (schedulePlanId < 1)
            {
                return BadRequest();
            }
            return Success(await schedulePlanBL.GetById(schedulePlanId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int schedulePlanId)
        {
            if (schedulePlanId < 1)
            {
                return BadRequest();
            }
            return Success(await schedulePlanBL.Delete(schedulePlanId));
        }
        [HttpGet]
        public async Task<ActionResult> GetSchedulePlansInformations()
        {
            return Success(await schedulePlanBL.GetSchedulePlansInformations());
        }
    }
}