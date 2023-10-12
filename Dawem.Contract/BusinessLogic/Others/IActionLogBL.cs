using Dawem.Models.Dtos.Others;
using Dawem.Models.Response;
using Dawem.Models.Response.Others;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Criteria.Others;

namespace SmartBusinessERP.BusinessLogic.Others.Contract
{
    public interface IActionLogBL
    {
        BaseResponseT<ActionLogDTO> GetById(int Id);
        GetActionLogsResponse Get(GetActionLogsCriteria criteria);
        Task<GetActionLogInfoResponse> GetInfo(GetActionLogInfoCriteria criteria);
        Task<BaseResponseT<bool>> Create(CreateActionLogModel model);
    }
}
