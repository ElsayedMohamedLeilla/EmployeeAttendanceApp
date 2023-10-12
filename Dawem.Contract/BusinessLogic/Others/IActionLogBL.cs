using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Criteria.Others;
using SmartBusinessERP.Models.Dtos.Others;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Models.Response.Others;

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
