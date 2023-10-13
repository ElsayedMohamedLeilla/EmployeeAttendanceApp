using Dawem.Models.Dtos.Provider;
using Dawem.Models.Response;

namespace Dawem.Contract.BusinessValidation
{
    public interface IRegisterationBLValidation
    {
        Task<BaseResponseT<RegisterResponseModel>> RegisterationValidator(RegisterModel ValidatorModel);

    }
}
