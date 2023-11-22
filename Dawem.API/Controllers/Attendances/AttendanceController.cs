using Dawem.Contract.BusinessLogic.Schedules.Schedules;
using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Schedules.Schedules
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class EmployeeAttendanceController : BaseController
    {
        private readonly IEmployeeAttendanceBL employeeAttendanceBL;

        public EmployeeAttendanceController(IEmployeeAttendanceBL _employeeAttendanceBL)
        {
            employeeAttendanceBL = _employeeAttendanceBL;
        }

        [HttpPost]
        public async Task<ActionResult> FingerPrint(FingerprintModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await employeeAttendanceBL.FingerPrint(model);
            var messageCode = result == Enums.Generals.FingerPrintType.Attendance ?
                 LeillaKeys.DoneCheckInSuccessfully : LeillaKeys.DoneCheckOutSuccessfully;
            return Success(result, messageCode: messageCode);
        }
        [HttpGet]
        public async Task<ActionResult> GetCurrentFingerPrintInfo()
        {
            var response = await employeeAttendanceBL.GetCurrentFingerPrintInfo();
            return Success(response);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetEmployeeAttendancesCriteria model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = await employeeAttendanceBL.GetEmployeeAttendances(model);
            return Success(response);
        }

    }
}