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
    public class SummonLogController : BaseController
    {
        private readonly ISummonLogBL summonLogBL;

        public SummonLogController(ISummonLogBL _summonLogBL)
        {
            summonLogBL = _summonLogBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSummonLogsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var summonLogsResponse = await summonLogBL.Get(criteria);

            return Success(summonLogsResponse.SummonLogs, summonLogsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int summonLogId)
        {
            if (summonLogId < 1)
            {
                return BadRequest();
            }
            return Success(await summonLogBL.GetInfo(summonLogId));
        }
        [HttpGet]
        public async Task<ActionResult> GetSummonLogsInformations()
        {
            return Success(await summonLogBL.GetSummonLogsInformations());
        }

    }
}