using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultTaskTypeController : AdminPanelControllerBase
    {
        private readonly IDefaultTaskTypeBL TaskTypeBL;
        public DefaultTaskTypeController(IDefaultTaskTypeBL _TaskTypeBL)
        {
            TaskTypeBL = _TaskTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultTasksTypeDTO model)
        {
            var result = await TaskTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateTaskTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultTasksTypeDTO model)
        {

            var result = await TaskTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateTaskTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultTaskTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await TaskTypeBL.Get(criteria);
            return Success(result.DefaultTasksTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultTaskTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await TaskTypeBL.GetForDropDown(criteria);
            return Success(result.DefaultTasksTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int TaskTypeId)
        {
            if (TaskTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await TaskTypeBL.GetInfo(TaskTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int TaskTypeId)
        {
            if (TaskTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await TaskTypeBL.GetById(TaskTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int TaskTypeId)
        {
            if (TaskTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await TaskTypeBL.Delete(TaskTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int TaskTypeId)
        {
            if (TaskTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await TaskTypeBL.Enable(TaskTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await TaskTypeBL.Disable(model));
        }

    }
}
