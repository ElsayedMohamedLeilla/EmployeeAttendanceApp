using Dawem.Data;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.SchedulePlanLogs;

namespace Dawem.Contract.Repository.Schedules.SchedulePlans
{
    public interface ISchedulePlanBackgroundJobLogRepository : IGenericRepository<SchedulePlanLog>
    {
        IQueryable<SchedulePlanLog> GetAsQueryable(GetSchedulePlanLogCriteria criteria);
    }
}
