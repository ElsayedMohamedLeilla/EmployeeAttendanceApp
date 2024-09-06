using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultPermissionTypeController : AdminPanelControllerBase
    {
        private readonly IDefaultPermissionTypeBL PermissionTypeBL;
        public DefaultPermissionTypeController(IDefaultPermissionTypeBL _PermissionTypeBL)
        {
            PermissionTypeBL = _PermissionTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultPermissionsTypeDTO model)
        {
            var result = await PermissionTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreatePermissionsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultPermissionsTypeDTO model)
        {

            var result = await PermissionTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdatePermissionsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultPermissionTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await PermissionTypeBL.Get(criteria);
            return Success(result.DefaultPermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultPermissionTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await PermissionTypeBL.GetForDropDown(criteria);
            return Success(result.DefaultPermissionsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int PermissionTypeId)
        {
            if (PermissionTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionTypeBL.GetInfo(PermissionTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int PermissionTypeId)
        {
            if (PermissionTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionTypeBL.GetById(PermissionTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int PermissionTypeId)
        {
            if (PermissionTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionTypeBL.Delete(PermissionTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int PermissionTypeId)
        {
            if (PermissionTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionTypeBL.Enable(PermissionTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await PermissionTypeBL.Disable(model));
        }

    }
}
