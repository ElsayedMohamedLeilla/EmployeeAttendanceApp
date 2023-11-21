using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IAuthenticationBL
    {
        Task<int> VerifyCompanyCode(string identityCode);
        Task<bool> RequestResetPassword(RequestResetPasswordModel model);
        Task<bool> ResetPassword(ResetPasswordModel model);
        Task<bool> SignUp(SignUpModel model);
        Task<bool> VerifyEmail(string token, string email);
        Task<TokenDto> SignIn(SignInModel model);
        Task<bool> ChangePassword(ChangePasswordModel model);
    }
}
