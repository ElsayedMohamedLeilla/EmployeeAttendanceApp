using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultShiftTypeController : AdminPanelControllerBase
    {
        private readonly IDefaultShiftTypeBL ShiftTypeBL;
        public DefaultShiftTypeController(IDefaultShiftTypeBL _ShiftTypeBL)
        {
            ShiftTypeBL = _ShiftTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultShiftsTypeDTO model)
        {
            var result = await ShiftTypeBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreateShiftsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultShiftsTypeDTO model)
        {

            var result = await ShiftTypeBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdatedShiftsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultShiftTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await ShiftTypeBL.Get(criteria);
            return Success(result.DefaultShiftsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultShiftTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await ShiftTypeBL.GetForDropDown(criteria);
            return Success(result.DefaultShiftsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int ShiftTypeId)
        {
            if (ShiftTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftTypeBL.GetInfo(ShiftTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int ShiftTypeId)
        {
            if (ShiftTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftTypeBL.GetById(ShiftTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int ShiftTypeId)
        {
            if (ShiftTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftTypeBL.Delete(ShiftTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int ShiftTypeId)
        {
            if (ShiftTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftTypeBL.Enable(ShiftTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftTypeBL.Disable(model));
        }

    }
}
