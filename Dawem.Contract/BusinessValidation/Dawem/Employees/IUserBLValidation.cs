using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface IUserBLValidation
    {
        Task<int?> SignUpValidation(UserSignUpModel model);
        Task<bool> VerifyEmailValidation(UserVerifyEmailModel model);
        Task<bool> SendVerificationCodeValidation(SendVerificationCodeModel model);
        Task<bool> CreateValidation(CreateUserModel model);
        Task<bool> UpdateValidation(UpdateUserModel model);
        Task<bool> AdminPanelCreateValidation(AdminPanelCreateUserModel model);
        Task<bool> AdminPanelUpdateValidation(AdminPanelUpdateUserModel model);
    }
}
