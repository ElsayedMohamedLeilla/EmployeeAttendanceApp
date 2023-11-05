using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Controllers.Employees
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class EmployeeController : BaseController
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
        public async Task<ActionResult> GetInfo([FromQuery] int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest();
            }
            return Success(await employeeBL.GetInfo(employeeId));
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
        [HttpDelete]
        public async Task<ActionResult> Delete(int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest();
            }
            return Success(await employeeBL.Delete(employeeId));
        }

    }
}