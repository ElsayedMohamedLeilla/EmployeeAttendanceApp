using Dawem.Contract.BusinessLogic.Dawem.Lookups;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Lookups;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Lookups;
using Dawem.Models.Dtos.Dawem.Lookups;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Translations;
using IPinfo;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dawem.BusinessLogic.Dawem.Lookups
{
    public class LookupsBL : ILookupsBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo userContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUploadBLC uploadBLC;
        private readonly IRepositoryManager repositoryManager;
        public LookupsBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IHttpContextAccessor _httpContextAccessor, IUploadBLC _uploadBLC, IRepositoryManager _repositoryManager,
             RequestInfo _userContext)
        {
            unitOfWork = _unitOfWork;
            httpContextAccessor = _httpContextAccessor;
            userContext = _userContext;
            uploadBLC = _uploadBLC;
            repositoryManager = _repositoryManager;
        }
        public async Task<List<CountryLiteDTO>> GetCountries(GetCountriesCriteria criteria)
        {
            var countryPredicate = PredicateBuilder.New<Country>(true);
            var countryRepository = repositoryManager.CountryRepository;

            var getCurrentCountryCode = await GetCurrentCountryInfo();

            if (criteria.Id is not 0 && criteria.Id is not null)
            {
                countryPredicate = countryPredicate.And(x => x.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                countryPredicate = countryPredicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                countryPredicate = countryPredicate.Start(x => x.NameAr.ToLower().Trim().Contains(criteria.FreeText));
                countryPredicate = countryPredicate.Or(x => x.NameEn.ToLower().Trim().Contains(criteria.FreeText));
            }

            #region paging

            //int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            //int take = PagingHelper.Take(criteria.PageSize);

            var query = countryRepository.Get(countryPredicate);

            #region sorting

            var queryOrdered = countryRepository.OrderBy(query, nameof(Country.Order), "asc");

            #endregion

            var queryPaged = queryOrdered;// criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var countries = await queryPaged.Select(c => new CountryLiteDTO()
            {
                Id = c.Id,
                Name = userContext.Lang == LeillaKeys.Ar ? c.NameAr : c.NameEn,
                CountryISOCode = c.Iso.ToLower(),
                IsCurrentCountry = c.Iso == getCurrentCountryCode ? true : null,
                Dial = LeillaKeys.PlusSign + LeillaKeys.Space + c.Dial,
                PhoneLength = c.PhoneLength + 1,
                FlagPath = uploadBLC.GetFilePath(c.Iso + LeillaKeys.PNG, LeillaKeys.AllCountriesFlags)
            }).ToListAsync();

            return countries;
        }
        public async Task<List<CurrencyLiteDTO>> GetCurrencies(GetCurrenciesCriteria criteria)
        {
            var currencyRepository = repositoryManager.CurrencyRepository;
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
        public async Task<string> GetCurrentCountryInfo()
        {
            var context = httpContextAccessor.HttpContext;
            var ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();

            if (ip.Contains(".0.0."))
            {
                ip = "41.47.113.144";
                //ip = "109.75.79.255";
            }

            string token = StaticGlobalVariables.IPInfoToken;
            IPinfoClient client = new IPinfoClient.Builder()
                .AccessToken(token)
                .Build();

            var ipResponse = await client.IPApi.GetDetailsAsync(ip);

            return ipResponse.Country;
        }
        public async Task<List<LanguageLiteDTO>> GetLanguages(GetLanguagesCriteria criteria)
        {
            var languageRepository = repositoryManager.LanguageRepository;
            var query = languageRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = languageRepository.OrderBy(query, nameof(Language.Order), LeillaKeys.Asc);

            #endregion


            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var currenciesList = await queryPaged.Select(c => new LanguageLiteDTO()
            {
                Id = c.Id,
                Name = c.NativeName,
                ISOCode = c.ISO2
            }).ToListAsync();

            return currenciesList;

        }
    }
}
