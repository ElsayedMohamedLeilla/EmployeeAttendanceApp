using Dawem.Contract.BusinessValidation;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Validation.BusinessValidation
{

    public class SignUpBLValidation : ISignUpBLValidation
    {

        public SignUpBLValidation()
        {

        }

        public bool SignUpValidation(SignUpModel model)
        {
            return true;
        }
    }
}
