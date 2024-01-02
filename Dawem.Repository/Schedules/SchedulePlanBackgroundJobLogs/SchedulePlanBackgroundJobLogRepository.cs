using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.SchedulePlanLogs;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Schedules.SchedulePlanBackgroundJobLogs
{
    public class SchedulePlanBackgroundJobLogRepository : GenericRepository<SchedulePlanLog>, ISchedulePlanBackgroundJobLogRepository
    {
        public SchedulePlanBackgroundJobLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<SchedulePlanLog> GetAsQueryable(GetSchedulePlanLogCriteria criteria)
        {
            var predicate = PredicateBuilder.New<SchedulePlanLog>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<SchedulePlanLog>(true);

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
