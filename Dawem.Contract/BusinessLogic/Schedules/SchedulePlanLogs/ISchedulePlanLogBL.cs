using Dawem.Models.Dtos.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.Response.Schedules.SchedulePlanLogs;

namespace Dawem.Contract.BusinessLogic.Schedules.SchedulePlanLogs
{
    public interface ISchedulePlanLogBL
    {
        Task<GetSchedulePlanLogInfoResponseModel> GetInfo(int schedulePlanBackgroudJobLogId);
        Task<GetSchedulePlanLogsResponse> Get(GetSchedulePlanLogCriteria model);
        Task<GetSchedulePlanLogEmployeesResponse> GetSchedulePlanLogEmployees(GetSchedulePlanLogEmployeesCriteria model);
    }
}
