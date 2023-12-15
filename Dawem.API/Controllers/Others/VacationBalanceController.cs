using Dawem.Contract.BusinessLogic.Schedules.VacationBalances;
using Dawem.Models.Dtos.Others.VacationBalances;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Schedules
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class VacationBalanceController : BaseController
    {
        private readonly IVacationBalanceBL schedulePlanBL;


        public VacationBalanceController(IVacationBalanceBL _schedulePlanBL)
        {
            schedulePlanBL = _schedulePlanBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create(CreateVacationBalanceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await schedulePlanBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateVacationBalancesSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update(UpdateVacationBalanceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await schedulePlanBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateVacationBalanceSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetVacationBalancesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var schedulePlansresponse = await schedulePlanBL.Get(criteria);

            return Success(schedulePlansresponse.VacationBalances, schedulePlansresponse.TotalCount);
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

    }
}