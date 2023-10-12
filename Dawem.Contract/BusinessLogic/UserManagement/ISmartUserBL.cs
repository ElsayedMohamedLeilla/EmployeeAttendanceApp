using SmartBusinessERP.Models.Criteria.UserManagement;
using SmartBusinessERP.Models.Dtos.Identity;
using SmartBusinessERP.Models.Dtos.Shared;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Identity;

namespace Dawem.Contract.BusinessLogic.UserManagement
{
    public interface ISmartUserBL
    {
        Task<SmartUserSearchResult> Get(SmartUserSearchCriteria criteria);
        Task<GetSmartUserInfoResponse> GetInfo(GetSmartUserInfoCriteria criteria);
        Task<BaseResponseT<CreatedUser>> Create(CreatedUser createdUser);


        BaseResponseT<bool> IsEmailUnique(ValidationItems validationItem);
        Task<BaseResponseT<CreatedUser>> Update(CreatedUser smartUserDto);
        Task<BaseResponseT<bool>> DeleteById(int userId);

    }
}
