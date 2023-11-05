using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Dtos.Employees.Attendance.ShiftWorkingTimes;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class ShiftWorkingTimeController : BaseController
    {
        private readonly IShiftWorkingTimeBL ShiftWorkingTimeBL;
        public ShiftWorkingTimeController(IShiftWorkingTimeBL _ShiftWorkingTimeBL)
        {
            ShiftWorkingTimeBL = _ShiftWorkingTimeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateShiftWorkingTimeModelDTO model)
        {
            var result = await ShiftWorkingTimeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateShiftWorkingTimeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateShiftWorkingTimeModelDTO model)
        {

            var result = await ShiftWorkingTimeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateShiftWorkingTimeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetShiftWorkingTimesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await ShiftWorkingTimeBL.Get(criteria);
            return Success(result.ShiftWorkingTimes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetShiftWorkingTimesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await ShiftWorkingTimeBL.GetForDropDown(criteria);
            return Success(result.ShiftWorkingTimes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int ShiftWorkingTimeId)
        {
            if (ShiftWorkingTimeId < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftWorkingTimeBL.GetInfo(ShiftWorkingTimeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int ShiftWorkingTimeId)
        {
            if (ShiftWorkingTimeId < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftWorkingTimeBL.GetById(ShiftWorkingTimeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int ShiftWorkingTimeId)
        {
            if (ShiftWorkingTimeId < 1)
            {
                return BadRequest();
            }
            return Success(await ShiftWorkingTimeBL.Delete(ShiftWorkingTimeId));
        }

    }
}
