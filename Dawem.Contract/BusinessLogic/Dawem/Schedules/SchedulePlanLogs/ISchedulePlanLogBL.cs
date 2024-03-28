using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.Response.Dawem.Schedules.SchedulePlanLogs;

namespace Dawem.Contract.BusinessLogic.Dawem.Schedules.SchedulePlanLogs
{
    public interface ISchedulePlanLogBL
    {
        Task<GetSchedulePlanLogInfoResponseModel> GetInfo(int schedulePlanBackgroudJobLogId);
        Task<GetSchedulePlanLogsResponse> Get(GetSchedulePlanLogCriteria model);
        Task<GetSchedulePlanLogEmployeesResponse> GetSchedulePlanLogEmployees(GetSchedulePlanLogEmployeesCriteria model);
    }
}
