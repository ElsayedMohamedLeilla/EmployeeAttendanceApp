using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.BusinessValidation
{
    public interface IAccountBLValidation
    {
        Task<bool> SignUpValidation(SignUpModel model);
        Task<MyUser> SignInValidation(SignInModel model);       
    }
}
