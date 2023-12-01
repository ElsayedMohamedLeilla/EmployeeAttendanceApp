using Dawem.Contract.BusinessLogic.Attendances;
using Dawem.Models.Dtos.Attendances;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Attendances
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
            var messageCode = model.Type == Enums.Generals.FingerPrintType.CheckIn ?
                 LeillaKeys.DoneCheckInSuccessfully : LeillaKeys.DoneCheckOutSuccessfully;
            return Success(await employeeAttendanceBL.FingerPrint(model), messageCode: messageCode);
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