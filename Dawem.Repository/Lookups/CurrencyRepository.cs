using Dawem.Contract.Repository.Lookups;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Generic;
using Dawem.Translations;
using LinqKit;
using SmartBusinessERP.Models.Criteria.Lookups;

namespace Dawem.Repository.Lookups
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
            

        }
        public IQueryable<Currency> GetAsQueryable(GetCurrenciesCriteria criteria, string includeProperties = DawemKeys.EmptyString)
        {
            var currencyPredicate = PredicateBuilder.New<Currency>(true);

            if (criteria.Id is not 0 && criteria.Id is not null)
            {
                currencyPredicate = currencyPredicate.And(x => x.Id == criteria.Id);
            }

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                currencyPredicate = currencyPredicate.Start(x => x.NameAr.ToLower().Trim().Contains(criteria.FreeText));
                currencyPredicate = currencyPredicate.Or(x => x.NameEn.ToLower().Trim().Contains(criteria.FreeText));
            }

            var query = Get(currencyPredicate, includeProperties: includeProperties);
            return query;
        }
    }
}
