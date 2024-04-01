using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Schedules.SchedulePlanBackgroundJobLogs
{
    public class SchedulePlanBackgroundJobLogRepository : GenericRepository<SchedulePlanLog>, ISchedulePlanBackgroundJobLogRepository
    {
        private readonly RequestInfo _requestInfo;
        public SchedulePlanBackgroundJobLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<SchedulePlanLog> GetAsQueryable(GetSchedulePlanLogCriteria criteria)
        {
            var predicate = PredicateBuilder.New<SchedulePlanLog>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<SchedulePlanLog>(true);

            predicate = predicate.And(e => e.CompanyId == _requestInfo.CompanyId);

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
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
