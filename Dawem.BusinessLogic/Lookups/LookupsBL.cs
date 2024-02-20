using Dawem.Contract.BusinessLogic.Lookups;
using Dawem.Contract.Repository.Lookups;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Lookups;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Lookups;
using Dawem.Models.Dtos.Lookups;
using Dawem.Translations;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dawem.BusinessLogic.Lookups
{
    public class LookupsBL : ILookupsBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly ICountryRepository countryRepository;
        private readonly ICurrencyRepository currencyRepository;
        private readonly RequestInfo userContext;
        public LookupsBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, ICurrencyRepository _currencyRepository, ICountryRepository _countryRepository,
             RequestInfo _userContext)
        {
            unitOfWork = _unitOfWork;
            currencyRepository = _currencyRepository;
            countryRepository = _countryRepository;
            userContext = _userContext;
        }

        public async Task<List<CountryLiteDTO>> GetCountries(GetCountriesCriteria criteria)
        {
            var countryPredicate = PredicateBuilder.New<Country>(true);


            if (criteria.Id is not 0 && criteria.Id is not null)
            {
                countryPredicate = countryPredicate.And(x => x.Id == criteria.Id);
            }

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                countryPredicate = countryPredicate.Start(x => x.NameAr.ToLower().Trim().Contains(criteria.FreeText));
                countryPredicate = countryPredicate.Or(x => x.NameEn.ToLower().Trim().Contains(criteria.FreeText));
            }

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            var query = countryRepository.Get(countryPredicate);

            #region sorting

            var queryOrdered = countryRepository.OrderBy(query, nameof(Country.Order), "asc");

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var countries = await queryPaged.Select(c => new CountryLiteDTO()
            {
                Id = c.Id,
                Name = userContext.Lang == LeillaKeys.Ar ? c.NameAr : c.NameEn,
                CountryISOCode = c.Iso.ToLower()
            }).ToListAsync();

            return countries;
        }
        public async Task<List<CurrencyLiteDTO>?> GetCurrencies(GetCurrenciesCriteria criteria)
        {
            var query = currencyRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = currencyRepository.OrderBy(query, nameof(Currency.Id), LeillaKeys.Asc);

            #endregion


            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var currenciesList = await queryPaged.Select(c => new CurrencyLiteDTO()
            {
                Id = c.Id,
                GlobalName = userContext.Lang == LeillaKeys.Ar ? c.NameAr : c.NameEn,
                CountryISOCode = c.Country.Iso.ToLower()
            }).ToListAsync();


            return currenciesList;

        }
    }
}
