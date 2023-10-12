using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartBusinessERP.BusinessLogic.Lookups.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Lookups;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Criteria.Lookups;
using SmartBusinessERP.Models.Dtos.Lookups;
using SmartBusinessERP.Models.Response.Lookups;
using SmartBusinessERP.Repository.Lookups.Contract;
using System.Data;

namespace SmartBusinessERP.BusinessLogic.Lookups
{
    public class LookupsBL : ILookupsBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly ICountryRepository countryRepository;
        private readonly ICurrencyRepository currencyRepository;
        private readonly RequestHeaderContext userContext;
        public LookupsBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, ICurrencyRepository _currencyRepository, ICountryRepository _countryRepository,
             RequestHeaderContext _userContext)
        {
            unitOfWork = _unitOfWork;
            currencyRepository = _currencyRepository;
            countryRepository = _countryRepository;
            userContext = _userContext;
        }

        public async Task<GetCountriesResponse> GetCountries(GetCountriesCriteria criteria)
        {

            GetCountriesResponse getCountriesResponse = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                ExpressionStarter<Country> countryPredicate = PredicateBuilder.New<Country>(true);


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

                var queryOrdered = countryRepository.OrderBy(query, "Id", "asc");

                #endregion

                var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                var countries = await queryPaged.Select(c => new CountryLiteDTO()
                {
                    Id = c.Id,
                    GlobalName = userContext.Lang == "ar" ? c.NameAr : c.NameEn,
                    CountryISOCode = c.Iso.ToLower()
                }).ToListAsync();


                getCountriesResponse.Countries = countries;
                getCountriesResponse.TotalCount = queryOrdered.ToList().Count();
                getCountriesResponse.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                getCountriesResponse.Exception = ex;
                getCountriesResponse.Status = ResponseStatus.Error;
            }

            return getCountriesResponse;

        }
        public async Task<GetCurrenciesResponse> GetCurrencies(GetCurrenciesCriteria criteria)
        {

            GetCurrenciesResponse response = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                ExpressionStarter<Currency> currencyPredicate = PredicateBuilder.New<Currency>(true);


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


                #region paging

                int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                int take = PagingHelper.Take(criteria.PageSize);

                var query = currencyRepository.Get(currencyPredicate);

                #region sorting

                var queryOrdered = currencyRepository.OrderBy(query, "Id", "asc");

                #endregion


                var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                var currenciesList = await queryPaged.Select(c => new CurrencyLiteDTO()
                {
                    Id = c.Id,
                    GlobalName = userContext.Lang == "ar" ? c.NameAr : c.NameEn,
                    CountryISOCode = c.Country.Iso.ToLower()
                }).ToListAsync();


                response.Currencies = currenciesList;
                response.TotalCount = queryOrdered.ToList().Count();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Exception = ex; response.Message = ex.Message;
                response.Status = ResponseStatus.Error;
            }

            return response;

        }
    }
}
