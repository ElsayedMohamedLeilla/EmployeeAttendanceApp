using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Summons
{
    public class SummonRepository : GenericRepository<Summon>, ISummonRepository
    {
        private readonly RequestInfo _requestInfo;
        public SummonRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<Summon> GetAsQueryable(GetSummonsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Summon>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Summon>(true);

            predicate = predicate.And(e => e.CompanyId == _requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.SummonSanctions.Any(a => a.Sanction.Name.ToLower().Trim().Contains(criteria.FreeText)));
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
