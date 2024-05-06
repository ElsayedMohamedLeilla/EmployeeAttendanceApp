using Dawem.Contract.Repository.Subscriptions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Dawem.Subscriptions.SubscriptionPayment;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class SubscriptionPaymentRepository : GenericRepository<SubscriptionPayment>, ISubscriptionPaymentRepository
    {
        public SubscriptionPaymentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {            
        }
        public IQueryable<SubscriptionPayment> GetAsQueryable(GetSubscriptionPaymentsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<SubscriptionPayment>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<SubscriptionPayment>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.Or(x => x.Subscription.Company.Name.ToLower().Trim().Contains(criteria.FreeText));
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
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
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
