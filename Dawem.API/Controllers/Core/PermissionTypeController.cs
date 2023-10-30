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
    public class PermissionTypeController : BaseController
    {
        private readonly IPermissionTypeBL permissionsTypeBL;
        public PermissionTypeController(IPermissionTypeBL _permissionTypeBL)
        {
            permissionsTypeBL = _permissionTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreatePermissionTypeDTO model)
        {
            var result = await permissionsTypeBL.Create(model);
            return Success(result, messageCode: DawemKeys.DoneCreatePermissionsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdatePermissionTypeDTO model)
        {

            var result = await permissionsTypeBL.Update(model);
            return Success(result, messageCode: DawemKeys.DoneUpdatePermissionsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetPermissionTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await permissionsTypeBL.Get(criteria);
            return Success(result.PermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetPermissionTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await permissionsTypeBL.GetForDropDown(criteria);
            return Success(result.PermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int permissionsTypeId)
        {
            if (permissionsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionsTypeBL.GetInfo(permissionsTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int permissionsTypeId)
        {
            if (permissionsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionsTypeBL.GetById(permissionsTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int permissionsTypeId)
        {
            if (permissionsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionsTypeBL.Delete(permissionsTypeId));
        }

    }
}
