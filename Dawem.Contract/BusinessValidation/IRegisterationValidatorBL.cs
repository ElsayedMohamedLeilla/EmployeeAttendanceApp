using Dawem.Models.Dtos.Provider;
using Dawem.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Validators.Contract
{
    public interface IRegisterationValidatorBL
    {
        Task<BaseResponseT<RegisterResponseModel>> RegisterationValidator(RegisterModel ValidatorModel);

    }
}
