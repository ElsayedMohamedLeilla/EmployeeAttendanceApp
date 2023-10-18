using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IAccountBL
    {
        Task<bool> ForgetPassword(ForgetPasswordModel model);
        Task<bool> SignUp(SignUpModel model);
        Task<bool> VerifyEmail(string token, string email);
        Task<TokenDto> SignIn(SignInModel model);
        Task<bool> ChangePassword(ChangePasswordModel model);
    }
}
