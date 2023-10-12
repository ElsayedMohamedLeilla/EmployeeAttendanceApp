using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Response;
using Dawem.Models.Response.Provider;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IAccountBL
    {
        Task<BaseResponseT<bool>> ForgetPassword(ForgetPasswordBindingModel forgetPasswordBindingModel);
        Task<BaseResponseT<RegisterResponseModel>> RegisterBasic(RegisterModel model);
        Task<BaseResponseT<bool>> VerifyEmail(string token, string email);
        Task<SignInResponse> SignIn(SignInModel authModel);

        Task<BaseResponseT<RegisterResponseModel>> ChangePassword(ChangePasswordBindingModel resetPasswordModel);
    }
}
