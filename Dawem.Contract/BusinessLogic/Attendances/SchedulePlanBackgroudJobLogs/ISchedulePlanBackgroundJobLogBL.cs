using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Attendances.Schedules;

namespace Dawem.Contract.BusinessLogic.Attendances.SchedulePlanBackgroudJobLogs
{
    public interface ISchedulePlanBackgroundJobLogBL
    {
        Task<GetSchedulePlanBackgroundJobLogInfoResponseModel> GetInfo(int schedulePlanBackgroudJobLogId);
        Task<GetSchedulePlanBackgroundJobLogsResponse> Get(GetSchedulePlanBackgroundJobLogsCriteria model);
    }
}
