using Dawem.Contract.Repository.Permissions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Permissions.Permissions;
using LinqKit;

namespace Dawem.Repository.Others
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly RequestInfo requestInfo;
        public PermissionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _requestInfo) : base(unitOfWork)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<Permission> GetAsQueryable(GetPermissionsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Permission>(a => !a.IsDeleted && a.IsActive);
            var inner = PredicateBuilder.New<Permission>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.PermissionScreens.Any(ps => TranslationHelper.GetTranslation(ps.ScreenCode.ToString(), requestInfo.Lang).ToLower().Trim().Contains(criteria.FreeText)));
                inner = inner.And(x => x.Role != null && x.Role.Name.ToLower().Trim().Contains(criteria.FreeText));
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
            if (criteria.ScreenCode != null)
            {
                predicate = predicate.And(e => e.PermissionScreens.Any(ps => ps.ScreenCode == criteria.ScreenCode));
            }
            if (criteria.ActionCode != null)
            {
                predicate = predicate.And(e => e.PermissionScreens.Any(ps => ps.PermissionScreenActions.Any(psa => psa.ActionCode == criteria.ActionCode)));
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }

}
