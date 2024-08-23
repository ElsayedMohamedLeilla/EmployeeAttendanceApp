using Dawem.BusinessLogic.Dawem.Core.Groups;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]    
    public class DefaultVacationTypeController : DawemControllerBase
    {
        private readonly IDefaultVacationTypeBL vacationTypeBL;
        public DefaultVacationTypeController(IDefaultVacationTypeBL _vacationTypeBL)
        {
            vacationTypeBL = _vacationTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultVacationsTypeDTO model)
        {
            var result = await vacationTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateVacationsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultVacationsTypeDTO model)
        {

            var result = await vacationTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateVacationsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultVacationTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await vacationTypeBL.Get(criteria);
            return Success(result.DefaultVacationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultVacationTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await vacationTypeBL.GetForDropDown(criteria);
            return Success(result.DefaultVacationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int vacationTypeId)
        {
            if (vacationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await vacationTypeBL.GetInfo(vacationTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int vacationTypeId)
        {
            if (vacationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await vacationTypeBL.GetById(vacationTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int vacationTypeId)
        {
            if (vacationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await vacationTypeBL.Delete(vacationTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int GroupId)
        {
            if (GroupId < 1)
            {
                return BadRequest();
            }
            return Success(await vacationTypeBL.Enable(GroupId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await vacationTypeBL.Disable(model));
        }

    }
}
