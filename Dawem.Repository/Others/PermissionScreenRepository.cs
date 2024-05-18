using Dawem.Contract.Repository.Permissions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Translations;
using LinqKit;

namespace Dawem.Repository.Others
{
    public class PermissionScreenRepository : GenericRepository<PermissionScreen>, IPermissionScreenRepository
    {
        private readonly RequestInfo requestInfo;
        public PermissionScreenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _requestInfo) : base(unitOfWork)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<PermissionScreen> GetAsQueryable(GetPermissionScreensCriteria criteria)
        {
            var predicate = PredicateBuilder.New<PermissionScreen>(a => !a.IsDeleted && a.IsActive);
            var inner = PredicateBuilder.New<PermissionScreen>(true);

            predicate = predicate.And(e => e.Permission.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(e => e.PermissionId == criteria.PermissionId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                var screenNameSuffix = requestInfo.Type == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

                var screenCodes = Enum.GetValues(typeof(DawemAdminApplicationScreenCode)).Cast<int>()
                    .ToList()
                    .Select(applicationScreenCode=> new
                    {
                        ScreenCode = applicationScreenCode,
                        ScreenName = TranslationHelper.GetTranslation(applicationScreenCode.ToString() + screenNameSuffix, requestInfo.Lang)
                    })
                    .ToList()
                    .Where(s=> s.ScreenName.ToLower().Trim().StartsWith(criteria.FreeText))
                    .Select(s=>s.ScreenCode)
                    .ToList();

                inner = inner.Or(ps => screenCodes != null && screenCodes.Contains(ps.ScreenCode));

                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.Id != null)
            {
                predicate = predicate.And(ps => ps.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }  
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(ps => ps.IsActive == criteria.IsActive);
            }
            if (criteria.ScreenCode != null)
            {
                predicate = predicate.And(ps => ps.ScreenCode == criteria.ScreenCode);
            }
            if (criteria.ActionCode != null)
            {
                predicate = predicate.And(ps => ps.PermissionScreenActions.Any(psa => psa.ActionCode == criteria.ActionCode));
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }

}
