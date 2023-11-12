using Dawem.Data;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.Repository.Attendances.Schedules
{
    public interface ISchedulePlanRepository : IGenericRepository<SchedulePlan>
    {
        IQueryable<SchedulePlan> GetAsQueryable(GetSchedulePlansCriteria criteria);
    }
}
