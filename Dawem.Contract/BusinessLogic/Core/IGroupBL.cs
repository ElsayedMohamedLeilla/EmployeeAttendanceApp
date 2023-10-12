using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Core;
using Dawem.Models.Response;
using Dawem.Models.Response.Core;
using SmartBusinessERP.Models.Criteria.Core;

namespace SmartBusinessERP.BusinessLogic.Core.Contract
{
    public interface IGroupBL
    {
        Task<BaseResponseT<GroupDTO>> GetById(int Id);
        Task<GetGroupsResponse> Get(GetGroupsCriteria criteria);
        Task<GetGroupInfoResponse> GetInfo(GetGroupInfoCriteria criteria);
        Task<BaseResponseT<Group>> Create(Group group);
        Task<BaseResponseT<bool>> Update(Group group);
        Task<BaseResponseT<bool>> Delete(int Id);
    }
}
