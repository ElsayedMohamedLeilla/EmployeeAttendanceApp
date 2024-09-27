using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class ResponsibilityController : DawemControllerBase
    {
        private readonly IResponsibilityBL responsibilityBL;
        public ResponsibilityController(IResponsibilityBL _responsibilityBL)
        {
            responsibilityBL = _responsibilityBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateResponsibilityModel model)
        {
            var result = await responsibilityBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateResponsibilitySuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateResponsibilityModel model)
        {

            var result = await responsibilityBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateResponsibilitySuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetResponsibilitiesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await responsibilityBL.Get(criteria);
            return Success(result.Responsibilities, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetResponsibilitiesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await responsibilityBL.GetForDropDown(criteria);
            return Success(result.Responsibilities, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int responsibilityId)
        {
            if (responsibilityId < 1)
            {
                return BadRequest();
            }
            return Success(await responsibilityBL.GetInfo(responsibilityId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int responsibilityId)
        {
            if (responsibilityId < 1)
            {
                return BadRequest();
            }
            return Success(await responsibilityBL.GetById(responsibilityId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int responsibilityId)
        {
            if (responsibilityId < 1)
            {
                return BadRequest();
            }
            return Success(await responsibilityBL.Delete(responsibilityId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int responsibilityId)
        {
            if (responsibilityId < 1)
            {
                return BadRequest();
            }
            return Success(await responsibilityBL.Enable(responsibilityId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await responsibilityBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetResponsibilitiesInformations()
        {
            return Success(await responsibilityBL.GetResponsibilitiesInformations());
        }
    }
}
