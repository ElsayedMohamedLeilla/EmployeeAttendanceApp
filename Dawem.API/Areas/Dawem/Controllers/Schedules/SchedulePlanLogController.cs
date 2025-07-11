﻿using Dawem.Contract.BusinessLogic.Dawem.Schedules.SchedulePlanLogs;
using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Schedules
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    
    
    public class SchedulePlanLogController : DawemControllerBase
    {
        private readonly ISchedulePlanLogBL schedulePlanLogBL;


        public SchedulePlanLogController(ISchedulePlanLogBL _schedulePlanLogBL)
        {
            schedulePlanLogBL = _schedulePlanLogBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSchedulePlanLogCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }

            var schedulePlanLogResponse = await schedulePlanLogBL.Get(criteria);
            return Success(schedulePlanLogResponse.SchedulePlanLogs, schedulePlanLogResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int schedulePlanLogId)
        {
            if (schedulePlanLogId < 1)
            {
                return BadRequest();
            }

            return Success(await schedulePlanLogBL.GetInfo(schedulePlanLogId));
        }
        [HttpGet]
        public async Task<ActionResult> GetSchedulePlanLogEmployees([FromQuery] GetSchedulePlanLogEmployeesCriteria model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            return Success(await schedulePlanLogBL.GetSchedulePlanLogEmployees(model));
        }
    }
}