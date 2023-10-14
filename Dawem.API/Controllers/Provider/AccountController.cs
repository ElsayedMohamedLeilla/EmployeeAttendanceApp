using Dawem.API.Controllers;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Dtos.Shared;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Provider
{
    [Route(DawemKeys.ApiCcontrollerAction)]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountBL accountBL;
        private readonly IMailBL mailBL;

        public AccountController(IAccountBL _accountBL, IMailBL _mailBL)
        {
            accountBL = _accountBL;
            mailBL = _mailBL;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(SignUpModelOld model)
        {
            return Success(await accountBL.RegisterBasic(model));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            return Success(await accountBL.SignIn(signInModel));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendVerificationCode(VerifyEmailModel model)
        {
            return Success(await mailBL.SendEmail(model));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string emailtoken, string email)
        {
            var response = await accountBL.VerifyEmail(emailtoken, email);

            if (response.Result == false)
            {
                return Redirect("https://www.google.com");
            }

            return Redirect("http://135.125.138.61/sessions/done-verify-account-successfully");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordBindingModel forgetPasswordBindingModel)
        {
            if (forgetPasswordBindingModel == null)
            {
                return BadRequest();

            }

            var response = await accountBL.ForgetPassword(forgetPasswordBindingModel);

            if (response.Result == false)
            {
                return Redirect("https://www.google.com");
            }

            return Redirect("https://www.SmartBusiness.com");
        }



        [HttpPost]
        public async Task<ActionResult> ResetPassword(ChangePasswordBindingModel resetPasswordModel)
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
            return Success(result);
        }

    }
}