using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Core
{
    public class ZoneRepository : GenericRepository<Zone>, IZoneRepository
    {
        private readonly RequestInfo _requestInfo;
        public ZoneRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }

        public IQueryable<Zone> GetAsQueryable(GetZoneCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Zone>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Zone>(true);

            predicate = predicate.And(e => e.CompanyId == _requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
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
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
            }
            if (criteria.Longitude != null && criteria.Latitude != null)
            {
                predicate = predicate.And(e => e.Longitude == criteria.Longitude && e.Latitude == criteria.Latitude);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
