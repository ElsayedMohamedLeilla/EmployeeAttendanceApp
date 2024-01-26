using Dawem.Contract.Repository.Attendances;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances.FingerprintDevices;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Attendances
{
    public class FingerprintDeviceRepository : GenericRepository<FingerprintDevice>, IFingerprintDeviceRepository
    {
        private readonly RequestInfo requestInfo;
        public FingerprintDeviceRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<FingerprintDevice> GetAsQueryable(GetFingerprintDevicesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<FingerprintDevice>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<FingerprintDevice>(true);

            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
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

            predicate = predicate.And(inner);

            var Query = Get(predicate);
            return Query;
        }
    }
}
