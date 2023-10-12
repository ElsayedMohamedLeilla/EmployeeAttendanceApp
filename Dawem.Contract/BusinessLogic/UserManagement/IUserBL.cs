using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Response;
using Dawem.Models.Response.Identity;
using SmartBusinessERP.Models.Criteria.UserManagement;

namespace Dawem.Contract.BusinessLogic.UserManagement
{
    public interface IUserBL
    {
        Task<UserSearchResult> Get(SmartUserSearchCriteria criteria);
        Task<GetUserInfoResponse> GetInfo(GetUserInfoCriteria criteria);
        Task<BaseResponseT<CreatedUser>> Create(CreatedUser createdUser);


        BaseResponseT<bool> IsEmailUnique(ValidationItems validationItem);
        Task<BaseResponseT<CreatedUser>> Update(CreatedUser smartUserDto);
        Task<BaseResponseT<bool>> DeleteById(int userId);

    }
}
