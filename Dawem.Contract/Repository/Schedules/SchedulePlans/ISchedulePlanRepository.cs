using Dawem.Data;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.SchedulePlans;

namespace Dawem.Contract.Repository.Schedules.SchedulePlans
{
    public interface ISchedulePlanRepository : IGenericRepository<SchedulePlan>
    {
        IQueryable<SchedulePlan> GetAsQueryable(GetSchedulePlansCriteria criteria);
    }
}
