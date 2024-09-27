using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Models.Dtos.Dawem.Identities;
using Dawem.Models.Dtos.Dawem.Providers;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Provider
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]    
    public class AuthenticationController : DawemControllerBase
    {
        private readonly IAuthenticationBL authenticationBL;
        private readonly IMailBL mailBL;

        public AuthenticationController(IAuthenticationBL _authenticationBL, IMailBL _mailBL)
        {
            authenticationBL = _authenticationBL;
            mailBL = _mailBL;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCompanyCode([FromQuery] string identityCode)
        {
            if (identityCode is null)
            {
                return BadRequest();
            }
            return Success(await authenticationBL.VerifyCompanyCode(identityCode), messageCode: LeillaKeys.DoneVerifyCompanyCodeSuccessfully);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(SignUpModel model)
        {
            return Success(await authenticationBL.SignUp(model), messageCode: LeillaKeys.DoneSignUpSuccessfullyCheckYourEmailToVerifyItAndLogIn);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            return Success(await authenticationBL.SignIn(signInModel), messageCode: LeillaKeys.DoneSignYouInSuccessfully);
        }
        [HttpPost]
        public new async Task<ActionResult> SignOut()
        {
            return Success(await authenticationBL.SignOut(), messageCode: LeillaKeys.DoneSignYouOutSuccessfully);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendVerificationCode(VerifyEmailModel model)
        {
            return Success(await mailBL.SendEmail(model));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string emailtoken, string email)
        {
            var response = await authenticationBL.VerifyEmail(emailtoken, email);

            if (response == false)
            {
                return Redirect("https://www.google.com");
            }
            return Redirect("https://stage.dawem.app/#/login");
        }
        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var response = await authenticationBL.ResetPassword(model);
            return Success(response, messageCode: LeillaKeys.DoneResetPasswordSuccessfully);
        }
        
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