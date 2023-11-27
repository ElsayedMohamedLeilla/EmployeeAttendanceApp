using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Zones;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class ZoneController : BaseController
    {
        private readonly IZoneBL ZoneBL;
        public ZoneController(IZoneBL _ZoneBL)
        {
            ZoneBL = _ZoneBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateZoneDTO model)
        {
            var result = await ZoneBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreateZoneSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateZoneDTO model)
        {

            var result = await ZoneBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdateZoneSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetZoneCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await ZoneBL.Get(criteria);
            return Success(result.Zones, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetZoneCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await ZoneBL.GetForDropDown(criteria);
            return Success(result.Zones, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await ZoneBL.GetInfo(ZoneId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await ZoneBL.GetById(ZoneId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await ZoneBL.Delete(ZoneId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await ZoneBL.Enable(ZoneId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await ZoneBL.Disable(model));
        }

    }
}
