using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Attendances
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize] 
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
                 LeillaKeys.DoneMakeSummonSuccessfully :
                 LeillaKeys.DoneCheckOutSuccessfully;
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
        public async Task<ActionResult> GetAttendances([FromQuery] GetEmployeeAttendancesForWebAdminCriteria model) // name will be change
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = await employeeAttendanceBL.GetEmployeeAttendancesForWebAdmin(model);
            return Success(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int employeeAttendanceId)
        {
            if (employeeAttendanceId == 0)
            {
                return BadRequest();
            }
            var response = await employeeAttendanceBL.GetEmployeeAttendancesInfo(employeeAttendanceId);
            return Success(response);
        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] DeleteEmployeeAttendanceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            return Success(await employeeAttendanceBL.Delete(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeesAttendancesInformations()
        {
            var response = await employeeAttendanceBL.GetEmployeesAttendancesInformations();
            return Success(response);
        }

        [HttpGet]
        public async Task<ActionResult> CreateExportDraft()
        {
            var stream = await employeeAttendanceBL.ExportDraft();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", AmgadKeys.EmployeeAttendanceEmptyDraft);
        }

        [HttpPost]
        [RequestSizeLimit(10 * 2048 * 2048)] // Max 20 MB
        public async Task<IActionResult> CreateImportDataFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(AmgadKeys.NoFileUploaded);
            }

            using Stream fileStream = file.OpenReadStream();
            Dictionary<string, string> result = await employeeAttendanceBL.ImportDataFromExcelToDB(fileStream);

            if (result.ContainsKey(AmgadKeys.Success))
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }

    }
}