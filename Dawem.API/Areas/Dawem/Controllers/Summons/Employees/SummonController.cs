using Dawem.Contract.BusinessLogic.Dawem.Summons;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Summons
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class EmployeeSummonController : DawemControllerBase
    {
        private readonly ISummonBL summonBL;

        public EmployeeSummonController(ISummonBL _summonBL)
        {
            summonBL = _summonBL;
        }
        
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSummonsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var summonsResponse = await summonBL.EmployeeGet(criteria);

            return Success(summonsResponse.Summons, summonsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int summonId)
        {
            if (summonId < 1)
            {
                return BadRequest();
            }
            return Success(await summonBL.EmployeeGetInfo(summonId));
        }
    }
}