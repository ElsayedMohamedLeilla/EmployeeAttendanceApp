using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Models.Response.Dawem.Employees.Users;

namespace Dawem.Contract.BusinessLogic.Dawem.Employees
{
    public interface IUserBL
    {
        Task<int> SignUp(UserSignUpModel model);
        Task<bool> VerifyEmail(UserVerifyEmailModel model);
        Task<bool> SendVerificationCode(SendVerificationCodeModel model);
        Task<int> Create(CreateUserModel model);
        Task<bool> Update(UpdateUserModel model);
        Task<int> AdminPanelCreate(AdminPanelCreateUserModel model);
        Task<bool> AdminPanelUpdate(AdminPanelUpdateUserModel model);
        Task<GetUserInfoResponseModel> GetInfo(int userId);
        Task<GetUserByIdResponseModel> GetById(int userId);
        Task<AdminPanelGetUserInfoResponseModel> AdminPanelGetInfo(int userId);
        Task<AdminPanelGetUserByIdResponseModel> AdminPanelGetById(int userId);
        Task<GetUsersResponse> Get(GetUsersCriteria model);
        Task<GetUsersForDropDownResponse> GetForDropDown(GetUsersCriteria model);
        Task<bool> Delete(int userId);
        Task<GetUsersInformationsResponseDTO> GetUsersInformations();
        Task<string> GetUserNameByEmployeeId(int employeeId);


    }
}
