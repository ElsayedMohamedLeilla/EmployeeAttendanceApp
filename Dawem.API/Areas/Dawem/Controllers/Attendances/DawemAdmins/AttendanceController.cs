using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Attendances.DawemAdmins
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class AttendanceController : DawemControllerBase
    {
        private readonly IEmployeeAttendanceBL employeeAttendanceBL;

        public AttendanceController(IEmployeeAttendanceBL _employeeAttendanceBL)
        {
            employeeAttendanceBL = _employeeAttendanceBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetEmployeeAttendancesForWebAdminCriteria model) // name will be change
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