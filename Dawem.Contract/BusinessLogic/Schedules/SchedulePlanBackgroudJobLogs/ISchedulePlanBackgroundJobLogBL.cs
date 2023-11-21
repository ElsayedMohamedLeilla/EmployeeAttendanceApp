using Dawem.Models.Dtos.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.Response.Schedules.SchedulePlanBackgroundJobLogs;

namespace Dawem.Contract.BusinessLogic.Schedules.SchedulePlanBackgroudJobLogs
{
    public interface ISchedulePlanBackgroundJobLogBL
    {
        Task<GetSchedulePlanBackgroundJobLogInfoResponseModel> GetInfo(int schedulePlanBackgroudJobLogId);
        Task<GetSchedulePlanBackgroundJobLogsResponse> Get(GetSchedulePlanBackgroundJobLogsCriteria model);
    }
}
