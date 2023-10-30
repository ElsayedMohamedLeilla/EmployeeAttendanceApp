using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Models.Dtos.Employees.TaskType;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Employees
{
    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class TaskTypeController : BaseController
    {
        private readonly ITaskTypeBL departmentBL;

        public TaskTypeController(ITaskTypeBL _departmentBL)
        {
            departmentBL = _departmentBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateTaskTypeModel model)
        {
            var result = await departmentBL.Create(model);
            return Success(result, messageCode: DawemKeys.DoneCreateTaskTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateTaskTypeModel model)
        {

            var result = await departmentBL.Update(model);
            return Success(result, messageCode: DawemKeys.DoneUpdateTaskTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetTaskTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await departmentBL.Get(criteria);

            return Success(departmensresponse.TaskTypes, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetTaskTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await departmentBL.GetForDropDown(criteria);

            return Success(departmensresponse.TaskTypes, departmensresponse.TotalCount);
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