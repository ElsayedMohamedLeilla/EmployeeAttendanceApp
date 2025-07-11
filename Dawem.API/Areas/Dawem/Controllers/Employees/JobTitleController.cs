﻿using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.JobTitles;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Employees
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    
    
    public class JobTitleController : DawemControllerBase
    {
        private readonly IJobTitleBL jobTitleBL;

        public JobTitleController(IJobTitleBL _jobTitleBL)
        {
            jobTitleBL = _jobTitleBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateJobTitleModel model)
        {
            var result = await jobTitleBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateJobTitleSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateJobTitleModel model)
        {

            var result = await jobTitleBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateJobTitleSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetJobTitlesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await jobTitleBL.Get(criteria);

            return Success(departmensresponse.JobTitles, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetJobTitlesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await jobTitleBL.GetForDropDown(criteria);

            return Success(departmensresponse.JobTitles, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int jobTitleId)
        {
            if (jobTitleId < 1)
            {
                return BadRequest();
            }
            return Success(await jobTitleBL.GetInfo(jobTitleId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int jobTitleId)
        {
            if (jobTitleId < 1)
            {
                return BadRequest();
            }
            return Success(await jobTitleBL.GetById(jobTitleId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int jobTitleId)
        {
            if (jobTitleId < 1)
            {
                return BadRequest();
            }
            return Success(await jobTitleBL.Delete(jobTitleId));
        }
        [HttpGet]
        public async Task<ActionResult> GetJobTitlesInformations()
        {
            return Success(await jobTitleBL.GetJobTitlesInformations());
        }
    }
}