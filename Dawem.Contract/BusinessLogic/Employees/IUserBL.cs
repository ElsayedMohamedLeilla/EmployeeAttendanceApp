
using Dawem.Models.Dtos.Employees.User;
using Dawem.Models.Response.Employees.User;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IUserBL
    {
        Task<int> SignUp(UserSignUpModel model);
        Task<bool> VerifyEmail(UserVerifyEmailModel model);
        Task<int> Create(CreateUserModel model);
        Task<bool> Update(UpdateUserModel model);
        Task<GetUserInfoResponseModel> GetInfo(int userId);
        Task<GetUserByIdResponseModel> GetById(int userId);
        Task<GetUsersResponse> Get(GetUsersCriteria model);
        Task<GetUsersForDropDownResponse> GetForDropDown(GetUsersCriteria model);
        Task<bool> Delete(int userId);
    }
}
