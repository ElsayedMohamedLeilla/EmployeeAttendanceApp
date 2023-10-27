using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Provider
{
    [Route(DawemKeys.ApiControllerAction)]
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
            return Success(result, messageCode: DawemKeys.DoneCreateDepartmentSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDepartmentModel model)
        {

            var result = await departmentBL.Update(model);
            return Success(result, messageCode: DawemKeys.DoneUpdateDepartmentSuccessfully);
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

    }
}