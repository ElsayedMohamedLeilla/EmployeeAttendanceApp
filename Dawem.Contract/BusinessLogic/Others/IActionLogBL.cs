using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Others;
using Dawem.Models.ResponseModels;

namespace Dawem.Contract.BusinessLogic.Others
{
    public interface IActionLogBL
    {
        Task<ActionLogDTO> GetById(int Id);
        Task<GetActionLogsResponseModel> Get(GetActionLogsCriteria criteria);
        Task<ActionLogInfo> GetInfo(GetActionLogInfoCriteria criteria);
        Task<bool> Create(CreateActionLogModel model);
    }
}
