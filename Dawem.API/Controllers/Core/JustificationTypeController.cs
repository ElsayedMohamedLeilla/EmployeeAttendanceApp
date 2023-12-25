using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(LeillaKeys.ApiControllerAction)] 
    [ApiController]
    [Authorize]
    public class JustificationTypeController : BaseController
    {
        private readonly IJustificationTypeBL justificationTypeBL;
        public JustificationTypeController(IJustificationTypeBL _justificationTypeBL)
        {
            justificationTypeBL = _justificationTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateJustificationsTypeDTO model)
        {
            var result = await justificationTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateJustificationsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateJustificationsTypeDTO model)
        {

            var result = await justificationTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateJustificationsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetJustificationsTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await justificationTypeBL.Get(criteria);
            return Success(result.JustificationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetJustificationsTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await justificationTypeBL.GetForDropDown(criteria);
            return Success(result.JustificationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int justificationTypeId)
        {
            if (justificationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await justificationTypeBL.GetInfo(justificationTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int justificationTypeId)
        {
            if (justificationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await justificationTypeBL.GetById(justificationTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int justificationTypeId)
        {
            if (justificationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await justificationTypeBL.Delete(justificationTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetJustificationTypesInformations()
        {
            return Success(await justificationTypeBL.GetJustificationTypesInformations());
        }
    }
}
