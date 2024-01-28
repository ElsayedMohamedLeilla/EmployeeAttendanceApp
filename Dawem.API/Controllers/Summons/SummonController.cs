using Dawem.Contract.BusinessLogic.Summons;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Summons
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class SummonController : BaseController
    {
        private readonly ISummonBL summonBL;

        public SummonController(ISummonBL _summonBL)
        {
            summonBL = _summonBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateSummonModel model)
        {
            var result = await summonBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateSummonSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateSummonModel model)
        {

            var result = await summonBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateSummonSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSummonsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var summonsResponse = await summonBL.Get(criteria);

            return Success(summonsResponse.Summons, summonsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int summonId)
        {
            if (summonId < 1)
            {
                return BadRequest();
            }
            return Success(await summonBL.GetInfo(summonId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int summonId)
        {
            if (summonId < 1)
            {
                return BadRequest();
            }
            return Success(await summonBL.GetById(summonId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int summonId)
        {
            if (summonId < 1)
            {
                return BadRequest();
            }
            return Success(await summonBL.Delete(summonId));
        }
        [HttpGet]
        public async Task<ActionResult> GetSummonsInformations()
        {
            return Success(await summonBL.GetSummonsInformations());
        }

    }
}