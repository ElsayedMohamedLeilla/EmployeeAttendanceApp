using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessValidation
{
    public interface ISignUpBLValidation
    {
        bool SignUpValidation(SignUpModel model);

    }
}
