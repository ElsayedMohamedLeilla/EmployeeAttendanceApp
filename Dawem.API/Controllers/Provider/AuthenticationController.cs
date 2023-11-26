using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Dtos.Shared;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Provider
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationBL authenticationBL;
        private readonly IMailBL mailBL;

        public AuthenticationController(IAuthenticationBL _authenticationBL, IMailBL _mailBL)
        {
            authenticationBL = _authenticationBL;
            mailBL = _mailBL;
        }
        [HttpGet]
        public async Task<ActionResult> VerifyCompanyCode([FromQuery] string identityCode)
        {
            if (identityCode is null)
            {
                return BadRequest();
            }
            return Success(await authenticationBL.VerifyCompanyCode(identityCode), messageCode: LeillaKeys.DoneVerifyCompanyCodeSuccessfully);
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpModel model)
        {
            return Success(await authenticationBL.SignUp(model), messageCode: LeillaKeys.DoneSignUpSuccessfullyCheckYourEmailToVerifyItAndLogIn);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            return Success(await authenticationBL.SignIn(signInModel), messageCode: LeillaKeys.DoneSignYouInSuccessfully);
        }
        [HttpPost]
        public async Task<ActionResult> SendVerificationCode(VerifyEmailModel model)
        {
            return Success(await mailBL.SendEmail(model));
        }
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string emailtoken, string email)
        {
            var response = await authenticationBL.VerifyEmail(emailtoken, email);

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
            var forgetPasswordResponse = await authenticationBL.RequestResetPassword(forgetPasswordBindingModel);
            return Success(forgetPasswordResponse, messageCode: LeillaKeys.DoneSendResetPasswordLinkToYourRegisteredEmailSuccessfully);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var response = await authenticationBL.ResetPassword(model);
            return Success(response, messageCode: LeillaKeys.DoneResetPasswordSuccessfully);
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
            var result = await authenticationBL.ChangePassword(resetPasswordModel);
            return Success(result, messageCode: LeillaKeys.DoneChangePasswordSuccessfully);
        }





    }
}