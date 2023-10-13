using Dawem.API.Controllers;
using Dawem.Models.Criteria.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBusinessERP.BusinessLogic.UserManagement.Contract;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Dtos.Identity;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Identity;
using SmartBusinessERP.Repository.UserManagement;

namespace Dawem.API.Controllers.UserManagement
{
    [Route(DawemKeys.ApiCcontrollerAction)]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly ISmartUserBL smartUserBL;
        private readonly SmartUserManagerRepository userManager;


        public UserController(ISmartUserBL _smartUserBL, SmartUserManagerRepository _userManager)
        {
            smartUserBL = _smartUserBL;
            userManager = _userManager;

        }

        [HttpPost]

        public async Task<ActionResult<SmartUserSearchResult>> Get(SmartUserSearchCriteria criteria)
        {

            if (criteria == null)
            {
                return BadRequest();
            }

            SmartUserSearchResult result = await smartUserBL.Get(criteria);

            return Ok(result);
        }

        [HttpPost]

        public async Task<ActionResult<GetSmartUserInfoResponse>> GetInfo(GetSmartUserInfoCriteria criteria)
        {

            if (criteria == null)
            {
                return BadRequest();
            }

            var result = await smartUserBL.GetInfo(criteria);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "FullAccess")]

        public async Task<ActionResult<BaseResponseT<CreatedUser>>> Create(CreatedUser createdUser)
        {

            if (createdUser == null)
            {
                return BadRequest();
            }

            Update(createdUser, InserationMode.Insert);



            BaseResponseT<CreatedUser> result = await smartUserBL.Create(createdUser);

            return Ok(result);
        }



        [HttpPost]
        [Authorize(Roles = "FullAccess")]
        public async Task<ActionResult<BaseResponseT<UpdatedUser>>> Update(CreatedUser updatedUser)
        {

            if (updatedUser == null)
            {
                return BadRequest();
            }


            Update(updatedUser, InserationMode.Update);

            BaseResponseT<CreatedUser> result = await smartUserBL.Update(updatedUser);

            return Ok(result);

        }



        [HttpPost]
        [Authorize()]
        public async Task<ActionResult<BaseResponseT<bool>>> DeleteUser([FromBody] int UserId)
        {

            if (!ModelState.IsValid || UserId <= 0)
            {
                return BadRequest();
            }
            BaseResponseT<bool> result = await smartUserBL.DeleteById(UserId);
            return Ok(result);
        }





    }
}
