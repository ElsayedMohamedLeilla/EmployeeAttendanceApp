using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.ResponseModels;

namespace Dawem.Contract.BusinessLogic.UserManagement
{
    public interface IUserBLOld
    {
        Task<GetUsersResponseModelOld> Get(UserSearchCriteria criteria);
        Task<UserInfo> GetInfo(GetUserInfoCriteria criteria);
        Task<int> Create(CreatedUser createdUser);
        Task<bool> IsEmailUnique(ValidationItems validationItem);
        Task<bool> Update(CreatedUser smartUserDto);
        Task<bool> DeleteById(int userId);
    }
}
