using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultDepartmentsController : AdminPanelControllerBase
    {
        private readonly IDefaultDepartmentsBL DepartmentsBL;
        public DefaultDepartmentsController(IDefaultDepartmentsBL _DepartmentsBL)
        {
            DepartmentsBL = _DepartmentsBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultDepartmentsDTO model)
        {
            var result = await DepartmentsBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateDepartmentSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultDepartmentsDTO model)
        {

            var result = await DepartmentsBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateDepartmentSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultDepartmentsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await DepartmentsBL.Get(criteria);
            return Success(result.DefaultDepartments, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultDepartmentsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await DepartmentsBL.GetForDropDown(criteria);
            return Success(result.DefaultDepartments, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int DepartmentId)
        {
            if (DepartmentId < 1)
            {
                return BadRequest();
            }
            return Success(await DepartmentsBL.GetInfo(DepartmentId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int DepartmentId)
        {
            if (DepartmentId < 1)
            {
                return BadRequest();
            }
            return Success(await DepartmentsBL.GetById(DepartmentId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int DepartmentId)
        {
            if (DepartmentId < 1)
            {
                return BadRequest();
            }
            return Success(await DepartmentsBL.Delete(DepartmentId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int DepartmentId)
        {
            if (DepartmentId < 1)
            {
                return BadRequest();
            }
            return Success(await DepartmentsBL.Enable(DepartmentId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await DepartmentsBL.Disable(model));
        }

    }
}
