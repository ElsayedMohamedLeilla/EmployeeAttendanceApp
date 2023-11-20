using Dawem.Data;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.SchedulePlanBackgroundJobLogs;

namespace Dawem.Contract.Repository.Schedules.SchedulePlans
{
    public interface ISchedulePlanBackgroundJobLogRepository : IGenericRepository<SchedulePlanBackgroundJobLog>
    {
        IQueryable<SchedulePlanBackgroundJobLog> GetAsQueryable(GetSchedulePlanBackgroundJobLogsCriteria criteria);
    }
}
