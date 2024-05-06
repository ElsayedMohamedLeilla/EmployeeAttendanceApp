using Dawem.Contract.BusinessLogic.Dawem.Others;
using Dawem.Models.Dtos.Dawem.Others.VacationBalances;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Others
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    
    
    public class VacationBalanceController : DawemControllerBase
    {
        private readonly IVacationBalanceBL vacationBalanceBL;


        public VacationBalanceController(IVacationBalanceBL _vacationBalanceBL)
        {
            vacationBalanceBL = _vacationBalanceBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create(CreateVacationBalanceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await vacationBalanceBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateVacationBalanceSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update(UpdateVacationBalanceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await vacationBalanceBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateVacationBalanceSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetVacationBalancesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var vacationBalancesresponse = await vacationBalanceBL.Get(criteria);

            return Success(vacationBalancesresponse.VacationBalances, vacationBalancesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int vacationBalanceId)
        {
            if (vacationBalanceId < 1)
            {
                return BadRequest();
            }
            return Success(await vacationBalanceBL.GetInfo(vacationBalanceId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int vacationBalanceId)
        {
            if (vacationBalanceId < 1)
            {
                return BadRequest();
            }
            return Success(await vacationBalanceBL.GetById(vacationBalanceId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int vacationBalanceId)
        {
            if (vacationBalanceId < 1)
            {
                return BadRequest();
            }
            return Success(await vacationBalanceBL.Delete(vacationBalanceId));
        }
        [HttpGet]
        public async Task<ActionResult> GetVacationBalancesInformations()
        {
            return Success(await vacationBalanceBL.GetVacationBalancesInformations());
        }
    }
}