using Dawem.Contract.Repository.Core.DefaultLookups;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Core.DefaultLookups
{
    public class DefaultVacationTypeRepository : GenericRepository<DefaultLookup>, IDefaultVacationTypeRepository
    {

        private readonly RequestInfo requestInfo;
        public DefaultVacationTypeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }

        public IQueryable<DefaultLookup> GetAsQueryable(GetDefaultVacationTypeCriteria criteria)
        {
            var predicate = PredicateBuilder.New<DefaultLookup>(a => a.LookupType == LookupsType.VacationsTypes);
            var inner = PredicateBuilder.New<DefaultLookup>(true);


            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.Start(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
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
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
