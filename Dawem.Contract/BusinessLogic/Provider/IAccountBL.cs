using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IAccountBL
    {
        Task<bool> ForgetPassword(ForgetPasswordBindingModel forgetPasswordBindingModel);
        Task<SignUpResponseModel> RegisterBasic(SignUpModelOld model);
        Task<bool> VerifyEmail(string token, string email);
        Task<TokenDto> SignIn(SignInModel authModel);
        Task<SignUpResponseModel> ChangePassword(ChangePasswordBindingModel resetPasswordModel);
    }
}
