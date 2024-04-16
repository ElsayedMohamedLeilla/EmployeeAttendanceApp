using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.UserManagement
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class UserController : DawemControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL _userBL)
        {
            userBL = _userBL;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(UserSignUpModel model)
        {
            return Success(await userBL.SignUp(model), messageCode: LeillaKeys.DoneSignUpSuccessfullyCheckYourEmailToVerifyItAndLogIn);
        }
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyEmail(UserVerifyEmailModel model)
        {
            return Success(await userBL.VerifyEmail(model), messageCode: LeillaKeys.DoneVerifyYourEmailSuccessfullyYouCanLogInNow);
        }
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> SendVerificationCode(SendVerificationCodeModel model)
        {
            return Success(await userBL.SendVerificationCode(model), messageCode: LeillaKeys.DoneSendVerificationCodeSuccessfully);
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromBody] CreateUserModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            //var model = JsonConvert.DeserializeObject<CreateUserModel>(model.CreateUserModelString);
            //model.ProfileImageFile = model.ProfileImageFile;
            var result = await userBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateUserSuccessfully);
        }
        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromBody] UpdateUserModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            //var model = JsonConvert.DeserializeObject<UpdateUserModel>(model.UpdateUserModelString);
            //model.ProfileImageFile = model.ProfileImageFile;
            var result = await userBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateUserSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetUsersCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var usersresponse = await userBL.Get(criteria);

            return Success(usersresponse.Users, usersresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetUsersCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var usersresponse = await userBL.GetForDropDown(criteria);

            return Success(usersresponse.Users, usersresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int userId)
        {
            if (userId < 1)
            {
                return BadRequest();
            }
            return Success(await userBL.GetInfo(userId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int userId)
        {
            if (userId < 1)
            {
                return BadRequest();
            }
            return Success(await userBL.GetById(userId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int userId)
        {
            if (userId < 1)
            {
                return BadRequest();
            }
            return Success(await userBL.Delete(userId));
        }
        [HttpGet]
        public async Task<ActionResult> GetUsersInformations()
        {
            return Success(await userBL.GetUsersInformations());
        }

        [HttpGet]
        public async Task<ActionResult> GetUserNameByEmployeeId([FromQuery]  int employeeId)
        {
            return Success(await userBL.GetUserNameByEmployeeId(employeeId));
        }

        
    }
}