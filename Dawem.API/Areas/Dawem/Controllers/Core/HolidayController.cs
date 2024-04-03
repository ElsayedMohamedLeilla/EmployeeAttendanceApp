using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.Holidays;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class HolidayController : DawemControllerBase
    {
        private readonly IHolidayBL holidayBL;
        public HolidayController(IHolidayBL _holidayBL)
        {
            holidayBL = _holidayBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateHolidayDTO model)
        {
            var result = await holidayBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreateHolidaySuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateHolidayDTO model)
        {

            var result = await holidayBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdateHolidaySuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetHolidayCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await holidayBL.Get(criteria);
            return Success(result.Holidaies, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetHolidayCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await holidayBL.GetForDropDown(criteria);
            return Success(result.Holidaies, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int HolidayId)
        {
            if (HolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await holidayBL.GetInfo(HolidayId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int HolidayId)
        {
            if (HolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await holidayBL.GetById(HolidayId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int HolidayId)
        {
            if (HolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await holidayBL.Delete(HolidayId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int HolidayId)
        {
            if (HolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await holidayBL.Enable(HolidayId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await holidayBL.Disable(model));
        }

        [HttpGet]
        public async Task<ActionResult> GetHolidaiesInformations()
        {
            var response = await holidayBL.GetHolidaiesInformation();
            return Success(response);
        }

    }
}
