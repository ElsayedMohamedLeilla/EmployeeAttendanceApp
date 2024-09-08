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
    public class DefaultOfficialHolidayTypeController : AdminPanelControllerBase
    {
        private readonly IDefaultOfficialHolidayTypeBL OfficialHolidayTypeBL;
        public DefaultOfficialHolidayTypeController(IDefaultOfficialHolidayTypeBL _OfficialHolidayTypeBL)
        {
            OfficialHolidayTypeBL = _OfficialHolidayTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultOfficialHolidaysDTO model)
        {
            var result = await OfficialHolidayTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateHolidayTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultOfficialHolidaysDTO model)
        {

            var result = await OfficialHolidayTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateHolidayTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultOfficialHolidayTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await OfficialHolidayTypeBL.Get(criteria);
            return Success(result.DefaultOfficialHolidaysTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultOfficialHolidayTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await OfficialHolidayTypeBL.GetForDropDown(criteria);
            return Success(result.DefaultOfficialHolidaysTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int OfficialHolidayTypeId)
        {
            if (OfficialHolidayTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayTypeBL.GetInfo(OfficialHolidayTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int OfficialHolidayTypeId)
        {
            if (OfficialHolidayTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayTypeBL.GetById(OfficialHolidayTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int OfficialHolidayTypeId)
        {
            if (OfficialHolidayTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayTypeBL.Delete(OfficialHolidayTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int OfficialHolidayTypeId)
        {
            if (OfficialHolidayTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayTypeBL.Enable(OfficialHolidayTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await OfficialHolidayTypeBL.Disable(model));
        }

    }
}
