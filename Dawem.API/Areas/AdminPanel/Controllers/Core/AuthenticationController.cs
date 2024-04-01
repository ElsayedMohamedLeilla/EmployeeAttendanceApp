using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Models.Dtos.Dawem.Identities;
using Dawem.Models.Dtos.Dawem.Providers;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Core
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, Authorize, AdminPanelAuthorize]
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
        [HttpPost]
        public async Task<ActionResult> SignIn(AdminPanelSignInModel signInModel)
        {
            return Success(await authenticationBL.AdminPanelSignIn(signInModel), messageCode: LeillaKeys.DoneSignYouInSuccessfully);
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