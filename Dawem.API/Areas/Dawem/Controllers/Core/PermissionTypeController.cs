using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    
    
    public class PermissionTypeController : DawemControllerBase
    {
        private readonly IPermissionTypeBL permissionTypeBL;
        public PermissionTypeController(IPermissionTypeBL _permissionTypeBL)
        {
            permissionTypeBL = _permissionTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreatePermissionTypeDTO model)
        {
            var result = await permissionTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreatePermissionsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdatePermissionTypeDTO model)
        {

            var result = await permissionTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdatePermissionsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetPermissionsTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await permissionTypeBL.Get(criteria);
            return Success(result.PermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetPermissionsTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await permissionTypeBL.GetForDropDown(criteria);
            return Success(result.PermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int permissionTypeId)
        {
            if (permissionTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionTypeBL.GetInfo(permissionTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int permissionTypeId)
        {
            if (permissionTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionTypeBL.GetById(permissionTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int permissionTypeId)
        {
            if (permissionTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await permissionTypeBL.Delete(permissionTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetPermissionTypesInformations()
        {
            return Success(await permissionTypeBL.GetPermissionTypesInformations());
        }
    }
}
