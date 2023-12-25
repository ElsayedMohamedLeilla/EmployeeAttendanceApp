using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Identities;
using Dawem.Models.Dtos.Providers;

namespace Dawem.Contract.BusinessValidation
{
    public interface IAccountBLValidation
    {
        Task<bool> SignUpValidation(SignUpModel model);
        Task<MyUser> SignInValidation(SignInModel model);       
    }
}
