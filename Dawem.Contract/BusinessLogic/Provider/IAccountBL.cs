using SmartBusinessERP.Models.Dtos.Identity;
using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Provider;

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
