using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.TaskTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Employees
{
    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
    public class TaskTypeController : BaseController
    {
        private readonly ITaskTypeBL taskTypeBL;

        public TaskTypeController(ITaskTypeBL _taskTypeBL)
        {
            taskTypeBL = _taskTypeBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateTaskTypeModel model)
        {
            var result = await taskTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateTaskTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateTaskTypeModel model)
        {

            var result = await taskTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateTaskTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetTaskTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await taskTypeBL.Get(criteria);

            return Success(departmensresponse.TaskTypes, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetTaskTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await taskTypeBL.GetForDropDown(criteria);

            return Success(departmensresponse.TaskTypes, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int taskTypeId)
        {
            if (taskTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await taskTypeBL.GetInfo(taskTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int taskTypeId)
        {
            if (taskTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await taskTypeBL.GetById(taskTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int taskTypeId)
        {
            if (taskTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await taskTypeBL.Delete(taskTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetTaskTypesInformations()
        {
            return Success(await taskTypeBL.GetTaskTypesInformations());
        }
    }
}