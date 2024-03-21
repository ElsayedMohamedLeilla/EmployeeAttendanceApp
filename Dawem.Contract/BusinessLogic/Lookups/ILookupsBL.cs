using Dawem.Models.Criteria.Lookups;
using Dawem.Models.Dtos.Lookups;

namespace Dawem.Contract.BusinessLogic.Lookups
{
    public interface ILookupsBL
    {
        Task<List<CountryLiteDTO>> GetCountries(GetCountriesCriteria criteria);
        Task<List<CurrencyLiteDTO>> GetCurrencies(GetCurrenciesCriteria criteria);
        Task<List<LanguageLiteDTO>> GetLanguages(GetLanguagesCriteria criteria);
    }
}
