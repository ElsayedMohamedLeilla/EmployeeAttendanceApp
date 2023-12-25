using Dawem.Models.Dtos.Employees.Users;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IUserBLValidation
    {
        Task<int?> SignUpValidation(UserSignUpModel model);
        Task<bool> VerifyEmailValidation(UserVerifyEmailModel model);
        Task<bool> SendVerificationCodeValidation(SendVerificationCodeModel model);
        Task<bool> CreateValidation(CreateUserModel model);
        Task<bool> UpdateValidation(UpdateUserModel model);
    }
}
