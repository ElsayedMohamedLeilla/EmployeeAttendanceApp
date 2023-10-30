using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class JustificationsTypeController : BaseController
    {
        private readonly IJustificationsTypeBL justificationsTypeBL;
        public JustificationsTypeController(IJustificationsTypeBL _justificationsTypeBL)
        {
            justificationsTypeBL = _justificationsTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateJustificationsTypeDTO model)
        {
            var result = await justificationsTypeBL.Create(model);
            return Success(result, messageCode: DawemKeys.DoneCreateJustificationsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateJustificationsTypeDTO model)
        {

            var result = await justificationsTypeBL.Update(model);
            return Success(result, messageCode: DawemKeys.DoneUpdateJustificationsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetJustificationsTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await justificationsTypeBL.Get(criteria);
            return Success(result.JustificationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetJustificationsTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await justificationsTypeBL.GetForDropDown(criteria);
            return Success(result.JustificationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int justificationsTypeId)
        {
            if (justificationsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await justificationsTypeBL.GetInfo(justificationsTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int justificationsTypeId)
        {
            if (justificationsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await justificationsTypeBL.GetById(justificationsTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int justificationsTypeId)
        {
            if (justificationsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await justificationsTypeBL.Delete(justificationsTypeId));
        }

    }
}
