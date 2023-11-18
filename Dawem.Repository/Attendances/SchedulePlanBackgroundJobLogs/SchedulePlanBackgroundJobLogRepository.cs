using Dawem.Contract.Repository.Attendances.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Attendances.Schedules
{
    public class SchedulePlanBackgroundJobLogRepository : GenericRepository<SchedulePlanBackgroundJobLog>, ISchedulePlanBackgroundJobLogRepository
    {
        public SchedulePlanBackgroundJobLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<SchedulePlanBackgroundJobLog> GetAsQueryable(GetSchedulePlanBackgroundJobLogsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<SchedulePlanBackgroundJobLog>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<SchedulePlanBackgroundJobLog>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.SchedulePlan.Schedule != null && x.SchedulePlan.Schedule.Name.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
