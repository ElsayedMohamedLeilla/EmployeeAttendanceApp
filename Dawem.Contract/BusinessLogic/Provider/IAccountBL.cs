using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IAccountBL
    {
        Task<bool> ForgetPassword(ForgetPasswordBindingModel forgetPasswordBindingModel);
        Task<RegisterResponseModel> RegisterBasic(RegisterModel model);
        Task<bool> VerifyEmail(string token, string email);
        Task<TokenDto> SignIn(SignInModel authModel);
        Task<RegisterResponseModel> ChangePassword(ChangePasswordBindingModel resetPasswordModel);
    }
}
