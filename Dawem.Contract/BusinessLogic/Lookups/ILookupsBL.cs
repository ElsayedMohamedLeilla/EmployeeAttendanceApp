using Dawem.Models.Dtos.Lookups;
using SmartBusinessERP.Models.Criteria.Lookups;

namespace SmartBusinessERP.BusinessLogic.Lookups.Contract
{
    public interface ILookupsBL
    {
        Task<List<CountryLiteDTO>> GetCountries(GetCountriesCriteria criteria);
        Task<List<CurrencyLiteDTO>> GetCurrencies(GetCurrenciesCriteria criteria);
    }
}
