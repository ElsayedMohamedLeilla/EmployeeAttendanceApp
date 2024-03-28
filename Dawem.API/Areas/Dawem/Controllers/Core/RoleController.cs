using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRoleBL RoleBL;
        public RoleController(IRoleBL _RoleBL)
        {
            RoleBL = _RoleBL;
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetRolesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await RoleBL.GetForDropDown(criteria);
            return Success(result.Roles, result.TotalCount);
        }
    }
}
