using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core;
using Dawem.Models.Response;
using Dawem.Models.Response.Core;

namespace Dawem.Contract.BusinessLogic.Core
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
