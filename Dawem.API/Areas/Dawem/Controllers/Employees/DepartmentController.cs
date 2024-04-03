using Dawem.Contract.BusinessLogic.Dawem.Employees.Department;
using Dawem.Models.Dtos.Dawem.Employees.Departments;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Employees
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class DepartmentController : DawemControllerBase
    {
        private readonly IDepartmentBL departmentBL;

        public DepartmentController(IDepartmentBL _departmentBL)
        {
            departmentBL = _departmentBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateDepartmentModel model)
        {
            var result = await departmentBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateDepartmentSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDepartmentModel model)
        {

            var result = await departmentBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateDepartmentSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDepartmentsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await departmentBL.Get(criteria);

            return Success(departmensresponse.Departments, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDepartmentsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await departmentBL.GetForDropDown(criteria);

            return Success(departmensresponse.Departments, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForTree([FromQuery] GetDepartmentsForTreeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await departmentBL.GetForTree(criteria);

            return Success(departmensresponse.Departments, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int departmentId)
        {
            if (departmentId < 1)
            {
                return BadRequest();
            }
            return Success(await departmentBL.GetInfo(departmentId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int departmentId)
        {
            if (departmentId < 1)
            {
                return BadRequest();
            }
            return Success(await departmentBL.GetById(departmentId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int departmentId)
        {
            if (departmentId < 1)
            {
                return BadRequest();
            }
            return Success(await departmentBL.Delete(departmentId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int departmentId)
        {
            if (departmentId < 1)
            {
                return BadRequest();
            }
            return Success(await departmentBL.Enable(departmentId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await departmentBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetDepartmentsInformations()
        {
            return Success(await departmentBL.GetDepartmentsInformations());
        }

        [HttpGet]
        public async Task<ActionResult> CreateExportDraft()
        {
            var stream = await departmentBL.ExportDraft();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", AmgadKeys.DepartmentEmptyDraft);
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
            Dictionary<string, string> result = await departmentBL.ImportDataFromExcelToDB(fileStream);

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