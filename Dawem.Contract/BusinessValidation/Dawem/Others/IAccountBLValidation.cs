using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Dawem.Identities;
using Dawem.Models.Dtos.Dawem.Providers;

namespace Dawem.Contract.BusinessValidation.Dawem.Others
{
    public interface IAccountBLValidation
    {
        Task<bool> SignUpValidation(SignUpModel model);
        Task<MyUser> SignInValidation(SignInModel model);
    }
}
