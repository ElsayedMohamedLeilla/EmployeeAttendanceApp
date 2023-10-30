using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.PermissionsTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class PermissionsTypeController : BaseController
    {
        private readonly IPermissionsTypeBL PermissionsTypeBL;
        public PermissionsTypeController(IPermissionsTypeBL _PermissionsTypeBL)
        {
            PermissionsTypeBL = _PermissionsTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreatePermissionsTypeDTO model)
        {
            var result = await PermissionsTypeBL.Create(model);
            return Success(result, messageCode: DawemKeys.DoneCreatePermissionsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdatePermissionsTypeDTO model)
        {

            var result = await PermissionsTypeBL.Update(model);
            return Success(result, messageCode: DawemKeys.DoneUpdatePermissionsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetPermissionsTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await PermissionsTypeBL.Get(criteria);
            return Success(result.PermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetPermissionsTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await PermissionsTypeBL.GetForDropDown(criteria);
            return Success(result.PermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int PermissionsTypeId)
        {
            if (PermissionsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionsTypeBL.GetInfo(PermissionsTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int PermissionsTypeId)
        {
            if (PermissionsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionsTypeBL.GetById(PermissionsTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int PermissionsTypeId)
        {
            if (PermissionsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionsTypeBL.Delete(PermissionsTypeId));
        }

    }
}
