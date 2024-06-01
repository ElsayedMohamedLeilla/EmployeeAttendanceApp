using Dawem.Contract.Repository.Settings;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        private readonly RequestInfo requestInfo;
        public MenuItemRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            this.requestInfo = requestInfo;
        }
        public IQueryable<MenuItem> GetAsQueryable(GetScreensCriteria criteria)
        {
            var predicate = PredicateBuilder.New<MenuItem>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<MenuItem>(true);

            //predicate = predicate.And(e => (requestInfo.AuthenticationType == AuthenticationType.AdminPanel && e.AuthenticationType == AuthenticationType.AdminPanel) || 
            //(requestInfo.AuthenticationType == AuthenticationType.DawemAdmin && e.AuthenticationType != AuthenticationType.AdminPanel));

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Or(x => x.MenuItemNameTranslations != null && x.MenuItemNameTranslations.Any(n => n.Name.StartsWith(criteria.FreeText)));

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
                predicate = predicate.And(e => e.MenuItemCode == criteria.ScreenCode);
            }
            if (criteria.ActionCode != null)
            {
                predicate = predicate.And(e => e.MenuItemActions != null && e.MenuItemActions.Any(a => (int)a.ActionCode == criteria.ActionCode));
            }

            if (criteria.LocalAuthenticationType != null)
            {
                predicate = predicate.And(e => e.AuthenticationType == criteria.LocalAuthenticationType);
            }

            else if (!criteria.ForGridView)
            {
                predicate = predicate.And(e => e.AuthenticationType == criteria.AuthenticationType);
            }

            if (criteria.ScreensIds != null && criteria.ScreensIds.Count > 0)
            {
                predicate = predicate.And(e => criteria.ScreensIds.Contains(e.Id));
            }
            if (!criteria.IsAllScreensAvailableInPlan && criteria.PlanId > 0)
            {
                predicate = predicate.And(e => e.GroupOrScreenType == GroupOrScreenType.Group ||
                e.PlanScreens.Any(p => p.PlanId == criteria.PlanId));
            }
            if (criteria.GroupOrScreenType != null)
            {
                predicate = predicate.And(e => e.GroupOrScreenType == criteria.GroupOrScreenType);
            }
            
            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
