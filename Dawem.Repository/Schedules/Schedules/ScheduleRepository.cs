using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Schedules.Schedules
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        private readonly RequestInfo requestInfo;
        public ScheduleRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<Schedule> GetAsQueryable(GetSchedulesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Schedule>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Schedule>(true);

            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
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
