using Dawem.API.Controllers;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Criteria.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBusinessERP.Models.Response.Others;

namespace Dawem.API.Controllers.Provider
{
    [Route(DawemKeys.ApiCcontrollerAction)]
    [ApiController]
    public class UserBranchController : BaseController
    {
        private readonly IUserBranchBL userbranchBL;

        public UserBranchController(IUserBranchBL _userbranchBL)
        {
            userbranchBL = _userbranchBL;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<GetUserBranchesResponse>> GetUserBranches(GetUserBranchCriteria criteria)
        {

            if (criteria == null)
            {
                return BadRequest();
            }

            var result = await userbranchBL.GetUserBranches(criteria);
            return Ok(result);
        }
    }
}