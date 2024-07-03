using Dawem.BusinessLogic.Dawem.Employees;
using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Attendances.Employees
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class EmployeeAttendanceController : DawemControllerBase
    {
        private readonly IEmployeeAttendanceBL employeeAttendanceBL;

        public EmployeeAttendanceController(IEmployeeAttendanceBL _employeeAttendanceBL)
        {
            employeeAttendanceBL = _employeeAttendanceBL;
        }
        [HttpPost]
        public async Task<ActionResult> CreateFingerPrint(FingerprintModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var fingerPrintType = await employeeAttendanceBL.CreateFingerPrint(model);
            var messageCode = fingerPrintType == FingerPrintType.CheckIn ?
                 LeillaKeys.DoneCheckInSuccessfully : fingerPrintType == FingerPrintType.Summon ?
                 LeillaKeys.DoneMakeSummonSuccessfully : fingerPrintType == FingerPrintType.BreakIn ?
                 LeillaKeys.DoneBreakInSuccessfully : fingerPrintType == FingerPrintType.BreakOut ?
                 LeillaKeys.DoneBreakOutSuccessfully : LeillaKeys.DoneCheckOutSuccessfully;
            return Success(fingerPrintType, messageCode: messageCode);
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
        [HttpGet]
        public async Task<ActionResult> GetCurrentEmployeeSchedules([FromQuery] GetCurrentEmployeeSchedulesModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = await employeeAttendanceBL.GetCurrentEmployeeSchedules(model);
            return Success(response, response.Schedules.Count);
        }

    }
}