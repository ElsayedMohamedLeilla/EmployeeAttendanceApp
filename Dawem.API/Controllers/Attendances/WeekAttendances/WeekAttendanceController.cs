using Dawem.Contract.BusinessLogic.WeekAttendances;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.WeekAttendances
{
    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class WeekAttendanceController : BaseController
    {
        private readonly IWeekAttendanceBL weekAttendanceBL;


        public WeekAttendanceController(IWeekAttendanceBL _weekAttendanceBL)
        {
            weekAttendanceBL = _weekAttendanceBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create(CreateWeekAttendanceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await weekAttendanceBL.Create(model);
            return Success(result, messageCode: DawemKeys.DoneCreateWeekAttendanceSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update(UpdateWeekAttendanceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await weekAttendanceBL.Update(model);
            return Success(result, messageCode: DawemKeys.DoneUpdateWeekAttendanceSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetWeekAttendancesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var weekAttendancesresponse = await weekAttendanceBL.Get(criteria);

            return Success(weekAttendancesresponse.WeekAttendances, weekAttendancesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetWeekAttendancesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var weekAttendancesresponse = await weekAttendanceBL.GetForDropDown(criteria);

            return Success(weekAttendancesresponse.WeekAttendances, weekAttendancesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int weekAttendanceId)
        {
            if (weekAttendanceId < 1)
            {
                return BadRequest();
            }
            return Success(await weekAttendanceBL.GetInfo(weekAttendanceId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int weekAttendanceId)
        {
            if (weekAttendanceId < 1)
            {
                return BadRequest();
            }
            return Success(await weekAttendanceBL.GetById(weekAttendanceId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int weekAttendanceId)
        {
            if (weekAttendanceId < 1)
            {
                return BadRequest();
            }
            return Success(await weekAttendanceBL.Delete(weekAttendanceId));
        }

    }
}