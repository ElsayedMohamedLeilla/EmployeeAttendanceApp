
using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Summons
{
    public class SummonLogRepository : GenericRepository<SummonLog>, ISummonLogRepository
    {
        private readonly RequestInfo _requestInfo;
        public SummonLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<SummonLog> GetAsQueryable(GetSummonLogsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<SummonLog>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<SummonLog>(true);

            predicate = predicate.And(e => e.CompanyId == _requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
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
            if (criteria.SummonDoneStatus != null)
            {
                switch (criteria.SummonDoneStatus.Value)
                {
                    case SummonDoneStatus.Done:
                        predicate = predicate.And(e => e.DoneSummon);
                        break;
                    case SummonDoneStatus.NotDone:
                        predicate = predicate.And(e => !e.DoneSummon);
                        break;
                    default:
                        break;
                }
            }
            if (criteria.SummonDate != null)
            {
                predicate = predicate.And(e => e.Summon.LocalDateAndTime.Date == criteria.SummonDate.Value.Date);
            }
            if (criteria.EmployeeNumber != null)
            {
                predicate = predicate.And(e => e.Employee.EmployeeNumber == criteria.EmployeeNumber.Value);
            }
            if (criteria.SummonCode != null)
            {
                predicate = predicate.And(e => e.Summon.Code == criteria.SummonCode.Value);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
