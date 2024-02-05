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
    public class SummonMissingLogController : BaseController
    {
        private readonly ISummonMissingLogBL summonMissingLogBL;

        public SummonMissingLogController(ISummonMissingLogBL _summonMissingLogBL)
        {
            summonMissingLogBL = _summonMissingLogBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSummonMissingLogsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var summonMissingLogsResponse = await summonMissingLogBL.Get(criteria);

            return Success(summonMissingLogsResponse.SummonMissingLogs, summonMissingLogsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int summonMissingLogId)
        {
            if (summonMissingLogId < 1)
            {
                return BadRequest();
            }
            return Success(await summonMissingLogBL.GetInfo(summonMissingLogId));
        }
        [HttpGet]
        public async Task<ActionResult> GetSummonMissingLogsInformations()
        {
            return Success(await summonMissingLogBL.GetSummonMissingLogsInformations());
        }

    }
}