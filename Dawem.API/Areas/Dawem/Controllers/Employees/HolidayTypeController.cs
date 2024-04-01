using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Employees
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class HolidayTypeController : BaseController
    {
        private readonly IHolidayTypeBL holidayTypeBL;

        public HolidayTypeController(IHolidayTypeBL _holidayTypeBL)
        {
            holidayTypeBL = _holidayTypeBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateHolidayTypeModel model)
        {
            var result = await holidayTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateHolidayTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateHolidayTypeModel model)
        {

            var result = await holidayTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateHolidayTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetHolidayTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await holidayTypeBL.Get(criteria);

            return Success(departmensresponse.HolidayTypes, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetHolidayTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await holidayTypeBL.GetForDropDown(criteria);

            return Success(departmensresponse.HolidayTypes, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int holidayTypeId)
        {
            if (holidayTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await holidayTypeBL.GetInfo(holidayTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int holidayTypeId)
        {
            if (holidayTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await holidayTypeBL.GetById(holidayTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int holidayTypeId)
        {
            if (holidayTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await holidayTypeBL.Delete(holidayTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetHolidayTypesInformations()
        {
            return Success(await holidayTypeBL.GetHolidayTypesInformations());
        }
    }
}