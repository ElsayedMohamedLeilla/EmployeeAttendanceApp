using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Dtos.Shared;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Provider
{
    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IAccountBL accountBL;
        private readonly IMailBL mailBL;

        public AccountController(IAccountBL _accountBL, IMailBL _mailBL)
        {
            accountBL = _accountBL;
            mailBL = _mailBL;
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpModel model)
        {
            return Success(await accountBL.SignUp(model), messageCode: DawemKeys.DoneSignUpSuccessfullyCheckYourEmailToVerifyItAndLogIn);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            return Success(await accountBL.SignIn(signInModel), messageCode: DawemKeys.DoneSignYouInSuccessfully);
        }
        [HttpPost]
        public async Task<ActionResult> SendVerificationCode(VerifyEmailModel model)
        {
            return Success(await mailBL.SendEmail(model));
        }
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string emailtoken, string email)
        {
            var response = await accountBL.VerifyEmail(emailtoken, email);

            if (response == false)
            {
                return Redirect("https://www.google.com");
            }

            return Redirect("https://www.youtube.com");
        }
        [HttpPost]
        public async Task<ActionResult> RequestResetPassword(RequestResetPasswordModel forgetPasswordBindingModel)
        {
            if (forgetPasswordBindingModel == null)
            {
                return BadRequest();

            }
            var forgetPasswordResponse = await accountBL.RequestResetPassword(forgetPasswordBindingModel);
            return Success(forgetPasswordResponse, messageCode: DawemKeys.DoneSendResetPasswordLinkToYourRegisteredEmailSuccessfully);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var response = await accountBL.ResetPassword(model);
            return Success(response, messageCode: DawemKeys.DoneResetPasswordSuccessfully);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel resetPasswordModel)
        {
            if (resetPasswordModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                var ret = GetExecutionResponseWithModelStateErrors();
                return Success(ret);
            }
            var result = await accountBL.ChangePassword(resetPasswordModel);
            return Success(result, messageCode: DawemKeys.DoneChangePasswordSuccessfully);
        }

    }
}