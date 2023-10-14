using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessValidation
{
    public interface IRegisterationBLValidation
    {
        bool RegisterationValidator(SignUpModel model);

    }
}
