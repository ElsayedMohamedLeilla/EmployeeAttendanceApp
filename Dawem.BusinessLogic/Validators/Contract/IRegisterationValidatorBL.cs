using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Validators.Contract
{
    public interface IRegisterationValidatorBL
    {
        Task<BaseResponseT<RegisterResponseModel>> RegisterationValidator(RegisterModel ValidatorModel);
    
    }
}
