using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultJobTitlesController : AdminPanelControllerBase
    {
        private readonly IDefaultJobTitlesBL JobTitlesBL;
        public DefaultJobTitlesController(IDefaultJobTitlesBL _JobTitlesBL)
        {
            JobTitlesBL = _JobTitlesBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultJobTitlesDTO model)
        {
            var result = await JobTitlesBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateJobTitleSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultJobTitlesDTO model)
        {

            var result = await JobTitlesBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateJobTitleSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultJobTitlesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await JobTitlesBL.Get(criteria);
            return Success(result.DefaultJobTitles, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultJobTitlesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await JobTitlesBL.GetForDropDown(criteria);
            return Success(result.DefaultJobTitles, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int JobTitleId)
        {
            if (JobTitleId < 1)
            {
                return BadRequest();
            }
            return Success(await JobTitlesBL.GetInfo(JobTitleId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int JobTitleId)
        {
            if (JobTitleId < 1)
            {
                return BadRequest();
            }
            return Success(await JobTitlesBL.GetById(JobTitleId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int JobTitleId)
        {
            if (JobTitleId < 1)
            {
                return BadRequest();
            }
            return Success(await JobTitlesBL.Delete(JobTitleId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int JobTitleId)
        {
            if (JobTitleId < 1)
            {
                return BadRequest();
            }
            return Success(await JobTitlesBL.Enable(JobTitleId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await JobTitlesBL.Disable(model));
        }

    }
}
