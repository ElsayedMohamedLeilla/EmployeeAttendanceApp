using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class OldRoleController : BaseController
    {
        private readonly IRoleBL RoleBL;
        public OldRoleController(IRoleBL _RoleBL)
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
