using Dawem.Contract.Repository.Settings;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class ScreenRepository : GenericRepository<Screen>, IScreenRepository
    {
        private readonly RequestInfo _requestInfo;
        public ScreenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<Screen> GetAsQueryable(GetScreensCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Screen>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Screen>(true);

            if (_requestInfo.Type == AuthenticationType.AdminPanel)
            {
                predicate = predicate.And(e => e.Type == _requestInfo.Type);
            }
            else if (_requestInfo.Type == AuthenticationType.DawemAdmin)
            {
                predicate = predicate.And(e => e.Type == _requestInfo.Type);
            }

            predicate = predicate.And(e => e.Type == _requestInfo.Type);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Or(x => x.ScreenNameTranslations != null && x.ScreenNameTranslations.Any(n => n.Name.StartsWith(criteria.FreeText)));

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
                predicate = predicate.And(e => e.ScreenCode == criteria.ScreenCode);
            }
            if (criteria.ActionCode != null)
            {
                predicate = predicate.And(e => e.ScreenActions != null && e.ScreenActions.Any(a => (int)a.ActionCode == criteria.ActionCode));
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
