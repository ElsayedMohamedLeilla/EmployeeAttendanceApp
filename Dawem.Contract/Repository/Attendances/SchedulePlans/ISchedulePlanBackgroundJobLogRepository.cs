using Dawem.Data;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.Repository.Attendances.SchedulePlans
{
    public interface ISchedulePlanBackgroundJobLogRepository : IGenericRepository<SchedulePlanBackgroundJobLog>
    {
        IQueryable<SchedulePlanBackgroundJobLog> GetAsQueryable(GetSchedulePlanBackgroundJobLogsCriteria criteria);
    }
}
