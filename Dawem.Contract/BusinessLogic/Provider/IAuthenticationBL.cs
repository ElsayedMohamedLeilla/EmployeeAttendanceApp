using Dawem.Models.Dtos.Identities;
using Dawem.Models.Dtos.Providers;

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
