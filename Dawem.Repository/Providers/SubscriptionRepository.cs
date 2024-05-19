using Dawem.Contract.Repository.Subscriptions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
        public IQueryable<Subscription> GetAsQueryable(GetSubscriptionsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Subscription>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Subscription>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Company.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Plan.PlanNameTranslations.Any(pn => pn.Name.ToLower().Trim().StartsWith(criteria.FreeText)));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
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
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(e => e.Status == criteria.Status);
            }
            if (criteria.Type is not null)
            {
                switch (criteria.Type.Value)
                {
                    case SubscriptionType.Subscription:
                        predicate = predicate.And(e => !e.Plan.IsTrial);
                        break;
                    case SubscriptionType.Trial:
                        predicate = predicate.And(e => e.Plan.IsTrial);
                        break;
                    default:
                        break;
                }
            }
            if (criteria.EndsAfterDaysFrom > 0)
            {
                var utcDate = DateTime.UtcNow.Date;
                predicate = predicate.And(e => (e.EndDate.Date - utcDate).TotalDays >= criteria.EndsAfterDaysFrom);
            }
            if (criteria.EndsAfterDaysTo > 0)
            {
                var utcDate = DateTime.UtcNow.Date;
                predicate = predicate.And(e => (e.EndDate.Date - utcDate).TotalDays <= criteria.EndsAfterDaysTo);
            }
            if (criteria.StartDateFrom is not null)
            {
                predicate = predicate.And(e => e.StartDate >= criteria.StartDateFrom);
            }
            if (criteria.StartDateTo is not null)
            {
                predicate = predicate.And(e => e.StartDate <= criteria.StartDateTo);
            }
            if (criteria.EndDateFrom is not null)
            {
                predicate = predicate.And(e => e.EndDate >= criteria.EndDateFrom);
            }
            if (criteria.EndDateTo is not null)
            {
                predicate = predicate.And(e => e.EndDate <= criteria.EndDateTo);
            }
            if (criteria.DurationInDaysFrom is not null)
            {
                predicate = predicate.And(e => e.DurationInDays >= criteria.DurationInDaysFrom);
            }
            if (criteria.DurationInDaysTo is not null)
            {
                predicate = predicate.And(e => e.DurationInDays <= criteria.DurationInDaysTo);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
