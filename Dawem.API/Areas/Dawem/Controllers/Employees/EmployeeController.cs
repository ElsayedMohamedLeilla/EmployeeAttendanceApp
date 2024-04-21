using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Employees
{
      [Route(LeillaKeys.DawemApiControllerAction), ApiController, 
        //Authorize, 
       // DawemAuthorize
        ]
    
    
    public class EmployeeController : DawemControllerBase
    {
        private readonly IEmployeeBL employeeBL;


        public EmployeeController(IEmployeeBL _employeeBL)
        {
            employeeBL = _employeeBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateEmployeeWithImageModel formData)
        {
            if (formData == null || formData.CreateEmployeeModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateEmployeeModel>(formData.CreateEmployeeModelString);
            model.ProfileImageFile = formData.ProfileImageFile;
            var result = await employeeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateEmployeeSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateEmployeeWithImageModel formData)
        {
            if (formData == null || formData.UpdateEmployeeModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateEmployeeModel>(formData.UpdateEmployeeModelString);
            model.ProfileImageFile = formData.ProfileImageFile;
            var result = await employeeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateEmployeeSuccessfully);
        }
        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> UpdateSpecificData([FromForm] UpdateEmployeeWithImageModel formData)
        {
            if (formData == null || formData.UpdateEmployeeModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateSpecificModelDTO>(formData.UpdateEmployeeModelString);
            model.ProfileImageFile = formData.ProfileImageFile;
            var result = await employeeBL.UpdateSpecificDataForEmployee(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateEmployeeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetEmployeesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var employeesresponse = await employeeBL.Get(criteria);
            return Success(employeesresponse.Employees, employeesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetEmployeesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var employeesresponse = await employeeBL.GetForDropDown(criteria);

            return Success(employeesresponse.Employees, employeesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDownEmployeeNotHaveUser([FromQuery] GetEmployeesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var employeesresponse = await employeeBL.GetForDropDownEmployeeNotHaveUser(criteria);

            return Success(employeesresponse.Employees, employeesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest();
            }
            return Success(await employeeBL.GetInfo(employeeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetCurrentEmployeeInfo()
        {
            return Success(await employeeBL.GetCurrentEmployeeInfo());
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest();
            }
            return Success(await employeeBL.GetById(employeeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest();
            }
            return Success(await employeeBL.Enable(employeeId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await employeeBL.Disable(model));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest();
            }
            return Success(await employeeBL.Delete(employeeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeesInformations()
        {
            return Success(await employeeBL.GetEmployeesInformations());
        }
        [HttpGet]
        public async Task<ActionResult> CreateExportDraft()
        {
            var stream = await employeeBL.ExportDraft();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", AmgadKeys.EmployeeEmptyDraft);
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
            Dictionary<string, string> result = await employeeBL.ImportDataFromExcelToDB(fileStream);

            if (result.ContainsKey(AmgadKeys.Success))
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetCurrentEmployeeSchedulePlanInCurrentMonth([FromQuery] GetEmployeeSchedulePlanCritria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var employeesresponse = await employeeBL.GetCurrentEmployeeShedulePlanInPeriod(criteria);
            return Success(employeesresponse.EmployeeSchedulePlan, employeesresponse.TotalCount);
        }



    }
}