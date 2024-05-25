using Dawem.Contract.Repository.Settings;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class ScreenGroupRepository : GenericRepository<ScreenGroup>, IScreenGroupRepository
    {
        private readonly RequestInfo _requestInfo;
        public ScreenGroupRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<ScreenGroup> GetAsQueryable(GetScreenGroupsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<ScreenGroup>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<ScreenGroup>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Or(x => x.ScreenGroupNameTranslations != null && x.ScreenGroupNameTranslations.Any(n => n.Name.StartsWith(criteria.FreeText)));

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

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
