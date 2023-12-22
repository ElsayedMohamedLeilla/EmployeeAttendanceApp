using Dawem.BusinessLogic.Employees.Departments;
using Dawem.Contract.BusinessLogic.Schedules.ShiftWorkingTime;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Schedules.ShiftWorkingTimes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Schedules.ShiftWorkingTimes
{

    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class ShiftWorkingTimeController : BaseController
    {
        private readonly IShiftWorkingTimeBL shiftWorkingTimeBL;
        public ShiftWorkingTimeController(IShiftWorkingTimeBL _ShiftWorkingTimeBL)
        {
            shiftWorkingTimeBL = _ShiftWorkingTimeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateShiftWorkingTimeModelDTO model)
        {
            var result = await shiftWorkingTimeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateShiftWorkingTimeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateShiftWorkingTimeModelDTO model)
        {

            var result = await shiftWorkingTimeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateShiftWorkingTimeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetShiftWorkingTimesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await shiftWorkingTimeBL.Get(criteria);
            return Success(result.ShiftWorkingTimes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetShiftWorkingTimesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await shiftWorkingTimeBL.GetForDropDown(criteria);
            return Success(result.ShiftWorkingTimes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int ShiftWorkingTimeId)
        {
            if (ShiftWorkingTimeId < 1)
            {
                return BadRequest();
            }
            return Success(await shiftWorkingTimeBL.GetInfo(ShiftWorkingTimeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int ShiftWorkingTimeId)
        {
            if (ShiftWorkingTimeId < 1)
            {
                return BadRequest();
            }
            return Success(await shiftWorkingTimeBL.GetById(ShiftWorkingTimeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int ShiftWorkingTimeId)
        {
            if (ShiftWorkingTimeId < 1)
            {
                return BadRequest();
            }
            return Success(await shiftWorkingTimeBL.Delete(ShiftWorkingTimeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int ShiftWorkingTimeId)
        {
            if (ShiftWorkingTimeId < 1)
            {
                return BadRequest();
            }
            return Success(await shiftWorkingTimeBL.Enable(ShiftWorkingTimeId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await shiftWorkingTimeBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetShiftWorkingTimesInformations()
        {
            return Success(await shiftWorkingTimeBL.GetShiftWorkingTimesInformations());
        }

    }
}
