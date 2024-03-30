using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Provider
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    public class UserBranchController : BaseController
    {
        private readonly IUserBranchBL userbranchBL;

        public UserBranchController(IUserBranchBL _userbranchBL)
        {
            userbranchBL = _userbranchBL;
        }
    }
}