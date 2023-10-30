using Dawem.Contract.BusinessLogic.UserManagement;
using Dawem.Enums.General;
using Dawem.Models.Context;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.UserManagement
{
    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class UserOldController : BaseController
    {
        private readonly IUserBLOld userBL;

        public UserOldController(IUserBLOld _smartUserBL, RequestInfo _requestHeaderContext) : base(_requestHeaderContext)
        {
            userBL = _smartUserBL;
        }

        [HttpPost]

        public async Task<ActionResult> Get(UserSearchCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }

            var result = await userBL.Get(criteria);
            return Success(result.Users, result.TotalCount);
        }

        [HttpPost]
        public async Task<ActionResult> GetInfo(GetUserInfoCriteria criteria)
        {

            if (criteria == null)
            {
                return BadRequest();
            }

            var result = await userBL.GetInfo(criteria);
            return Success(result);
        }

        [HttpPost]
        [Authorize(Roles = DawemKeys.FullAccess)]
        public async Task<ActionResult> Create(CreatedUser createdUser)
        {
            if (createdUser == null)
            {
                return BadRequest();
            }

            Update(createdUser, InserationMode.Insert);
            return Success(await userBL.Create(createdUser), messageCode: DawemKeys.DoneCreateUserSuccessfully);
        }
        [HttpPost]
        [Authorize(Roles = DawemKeys.FullAccess)]
        public async Task<ActionResult> Update(CreatedUser updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest();
            }
            Update(updatedUser, InserationMode.Update);
            return Success(await userBL.Update(updatedUser), messageCode: DawemKeys.DoneUpdateUserSuccessfully);

        }

        [HttpPost]
        [Authorize(Roles = DawemKeys.FullAccess)]
        public async Task<ActionResult> DeleteUser([FromBody] int UserId)
        {
            if (!ModelState.IsValid || UserId <= 0)
            {
                return BadRequest();
            }
            return Success(await userBL.DeleteById(UserId), messageCode: DawemKeys.DoneDeleteUserSuccessfully);
        }
    }
}
