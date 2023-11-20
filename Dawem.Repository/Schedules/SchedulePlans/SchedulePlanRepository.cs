using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Schedules.SchedulePlans
{
    public class SchedulePlanRepository : GenericRepository<SchedulePlan>, ISchedulePlanRepository
    {
        public SchedulePlanRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<SchedulePlan> GetAsQueryable(GetSchedulePlansCriteria criteria)
        {
            var predicate = PredicateBuilder.New<SchedulePlan>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<SchedulePlan>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.SchedulePlanEmployee != null && x.SchedulePlanEmployee.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));

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
