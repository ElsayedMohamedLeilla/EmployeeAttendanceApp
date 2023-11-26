using Dawem.Models.Dtos.Employees.TaskType;
using Dawem.Models.Dtos.Employees.User;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IUserBLValidation
    {
        Task<bool> SignUpValidation(UserSignUpModel model);
        Task<bool> VerifyEmailValidation(UserVerifyEmailModel model);
        Task<bool> CreateValidation(CreateUserModel model);
        Task<bool> UpdateValidation(UpdateUserModel model);
    }
}
