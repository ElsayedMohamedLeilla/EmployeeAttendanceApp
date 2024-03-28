using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Response.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Provider
{
    [Route(LeillaKeys.DawemApiControllerAction)]
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
            return Success(result.UserBranches, result.TotalCount);
        }
    }
}