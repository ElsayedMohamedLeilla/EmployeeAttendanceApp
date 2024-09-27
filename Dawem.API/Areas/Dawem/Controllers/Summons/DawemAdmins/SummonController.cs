using Dawem.Contract.BusinessLogic.Dawem.Summons;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Summons.DawemAdmins
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class SummonController : DawemControllerBase
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
        [HttpPut]
        public async Task<ActionResult> Enable(int summonId)
        {
            if (summonId < 1)
            {
                return BadRequest();
            }
            return Success(await summonBL.Enable(summonId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await summonBL.Disable(model));
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