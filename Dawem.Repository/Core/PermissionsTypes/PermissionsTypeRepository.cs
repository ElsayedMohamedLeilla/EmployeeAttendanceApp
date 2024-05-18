using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Core.PermissionsTypes
{
    public class PermissionsTypeRepository : GenericRepository<PermissionType>, IPermissionsTypeRepository
    {
        private readonly RequestInfo requestInfo;
        public PermissionsTypeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }

        public IQueryable<PermissionType> GetAsQueryable(GetPermissionsTypesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<PermissionType>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<PermissionType>(true);

            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

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
