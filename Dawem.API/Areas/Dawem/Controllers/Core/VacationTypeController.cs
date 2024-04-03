using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]    
    public class VacationTypeController : DawemControllerBase
    {
        private readonly IVacationTypeBL vacationTypeBL;
        public VacationTypeController(IVacationTypeBL _vacationTypeBL)
        {
            vacationTypeBL = _vacationTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateVacationsTypeDTO model)
        {
            var result = await vacationTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateVacationsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateVacationsTypeDTO model)
        {

            var result = await vacationTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateVacationsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetVacationsTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await vacationTypeBL.Get(criteria);
            return Success(result.VacationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetVacationsTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await vacationTypeBL.GetForDropDown(criteria);
            return Success(result.VacationsTypes, result.TotalCount);
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
        [HttpGet]
        public async Task<ActionResult> GetVacationTypesInformations()
        {
            return Success(await vacationTypeBL.GetVacationTypesInformations());
        }
    }
}
