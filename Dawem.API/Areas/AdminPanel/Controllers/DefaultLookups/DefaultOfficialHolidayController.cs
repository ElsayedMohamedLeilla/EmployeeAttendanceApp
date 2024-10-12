using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultOfficialHolidayController : AdminPanelControllerBase
    {
        private readonly IDefaultOfficialHolidayBL OfficialHolidayBL;
        public DefaultOfficialHolidayController(IDefaultOfficialHolidayBL _OfficialHolidayTypeBL)
        {
            OfficialHolidayBL = _OfficialHolidayTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultOfficialHolidaysDTO model)
        {
            var result = await OfficialHolidayBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateHolidayTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultOfficialHolidaysDTO model)
        {

            var result = await OfficialHolidayBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateHolidayTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultOfficialHolidayCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await OfficialHolidayBL.Get(criteria);
            return Success(result.DefaultOfficialHolidaysTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultOfficialHolidayCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await OfficialHolidayBL.GetForDropDown(criteria);
            return Success(result.DefaultOfficialHolidaysTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int officialHolidayId)
        {
            if (officialHolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayBL.GetInfo(officialHolidayId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int officialHolidayId)
        {
            if (officialHolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayBL.GetById(officialHolidayId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int officialHolidayId)
        {
            if (officialHolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayBL.Delete(officialHolidayId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int officialHolidayId)
        {
            if (officialHolidayId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayBL.Enable(officialHolidayId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayBL.Disable(model));
        }

    }
}
