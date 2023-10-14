using Dawem.Contract.BusinessValidation;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Validation.BusinessValidation
{

    public class RegisterationBLValidation : IRegisterationBLValidation
    {

        public RegisterationBLValidation()
        {

        }

        public bool RegisterationValidator(SignUpModel model)
        {
            return true;
        }
    }
}
