using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Models.Dtos.Employees.Users;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Controllers.UserManagement
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
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
        public async Task<ActionResult> Create([FromForm] CreateUserWithImageModel formData)
        {
            if (formData == null || formData.CreateUserModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateUserModel>(formData.CreateUserModelString);
            model.ProfileImageFile = formData.ProfileImageFile;
            var result = await userBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateUserSuccessfully);
        }
        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateUserWithImageModel formData)
        {
            if (formData == null || formData.UpdateUserModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateUserModel>(formData.UpdateUserModelString);
            model.ProfileImageFile = formData.ProfileImageFile;
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
    }
}