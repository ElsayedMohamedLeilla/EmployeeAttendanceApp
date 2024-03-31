using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlans;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Schedules.SchedulePlans
{
    public class SchedulePlanRepository : GenericRepository<SchedulePlan>, ISchedulePlanRepository
    {
        private readonly RequestInfo requestInfo;
        public SchedulePlanRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            this.requestInfo = requestInfo;
        }
        public IQueryable<SchedulePlan> GetAsQueryable(GetSchedulePlansCriteria criteria)
        {
            var predicate = PredicateBuilder.New<SchedulePlan>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<SchedulePlan>(true);

            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Schedule.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.And(x => x.SchedulePlanEmployee != null && x.SchedulePlanEmployee.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.And(x => x.SchedulePlanGroup != null && x.SchedulePlanGroup.Group.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.And(x => x.SchedulePlanDepartment != null && x.SchedulePlanDepartment.Department.Name.ToLower().Trim().Contains(criteria.FreeText));

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
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
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
