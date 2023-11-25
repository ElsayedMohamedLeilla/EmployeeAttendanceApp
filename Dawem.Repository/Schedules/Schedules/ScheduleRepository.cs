using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Schedules.Schedules
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<Schedule> GetAsQueryable(GetSchedulesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Schedule>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Schedule>(true);

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
