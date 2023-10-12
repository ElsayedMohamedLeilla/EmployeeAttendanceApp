using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Response;

using SmartBusinessERP.Helpers;
using SmartBusinessERP.Repository.Provider.Contract;
using SmartBusinessERP.BusinessLogic.Validators.Contract;
using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Repository.UserManagement.Contract;
using SmartBusinessERP.Repository.UserManagement;

namespace SmartBusinessERP.BusinessLogic.Validators
{

    public class RegisterationValidatorBL : IRegisterationValidatorBL
    {
        private readonly RequestHeaderContext userContext;
        private readonly IBranchRepository branchRepository;
        private readonly IUserBranchRepository userBranchRepository;
        private readonly ISmartUserRepository smartUserRepository;
        private readonly SmartUserManagerRepository smartUserManagerRepository;

        
        public RegisterationValidatorBL(RequestHeaderContext _userContext, IBranchRepository _branchRepository,
            IUserBranchRepository _userBranchRepository, ISmartUserRepository _smartUserRepository, SmartUserManagerRepository _smartUserManagerRepository)
        {

            userContext = _userContext;
            branchRepository = _branchRepository;
            userBranchRepository = _userBranchRepository;
            smartUserRepository = _smartUserRepository;
            smartUserManagerRepository = _smartUserManagerRepository;
        }

        public async Task<BaseResponseT<RegisterResponseModel>> RegisterationValidator(RegisterModel model)
        {
            var response = new BaseResponseT<RegisterResponseModel> ();



            if (model.UserEmail == null)
            {
                TranslationHelper.SetValidationMessages(response, "Account1002", "Missing email", lang: userContext.Lang);
                return response;
            }
            if (model.Password == null)
            {
                TranslationHelper.SetValidationMessages(response, "Account1003", "Missing password", lang: userContext.Lang);
                return response;
            }
            if (model.Password != model.ConfirmPassword)
            {
                TranslationHelper.SetValidationMessages(response, "Account1003", "Password and confirm password do not match.", lang: userContext.Lang);
                return response;
            }
          

            // mobile check
            var mobileExist = smartUserRepository.Get(c => c.MobileNumber == model.UserMobileNumber).FirstOrDefault();
            if (mobileExist != null)
            {
                TranslationHelper.SetValidationMessages(response, "Mobileisalreadytaken", "Mobile is already taken.", lang: userContext.Lang);
                return response;

            }
            try
            {
                var user = await smartUserManagerRepository.FindByEmailAsync(model.UserEmail);
                if (user != null)
                {
                    throw new Exception("");
                }
            }
            catch
            {
                response.Status = ResponseStatus.ValidationError;
                TranslationHelper.SetValidationMessages(response, "Emailisalreadytaken", "Email is already taken...", "", userContext.Lang);
                return response;
            }


            response.Status = ResponseStatus.Success;
            return response;
        }
    }
}
