using Dawem.Models.Criteria.Lookups;
using Dawem.Models.Dtos.Dawem.Lookups;

namespace Dawem.Contract.BusinessLogic.Dawem.Lookups
{
    public interface ILookupsBL
    {
        Task<List<CountryLiteDTO>> GetCountries(GetCountriesCriteria criteria);
        Task<List<CurrencyLiteDTO>> GetCurrencies(GetCurrenciesCriteria criteria);
        Task<List<LanguageLiteDTO>> GetLanguages(GetLanguagesCriteria criteria);
    }
}
