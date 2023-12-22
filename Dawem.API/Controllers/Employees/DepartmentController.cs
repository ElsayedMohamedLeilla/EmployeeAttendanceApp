using Dawem.BusinessLogic.Core.Groups;
using Dawem.Contract.BusinessLogic.Employees.Department;
using Dawem.Models.Dtos.Employees.Department;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Employees
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class DepartmentController : BaseController
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

    }
}