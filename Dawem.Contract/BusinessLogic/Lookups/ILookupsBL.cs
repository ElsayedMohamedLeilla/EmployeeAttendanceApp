using Dawem.Models.Response.Lookups;
using SmartBusinessERP.Models.Criteria.Lookups;

namespace SmartBusinessERP.BusinessLogic.Lookups.Contract
{
    public interface ILookupsBL
    {
        Task<GetCountriesResponse> GetCountries(GetCountriesCriteria criteria);
        Task<GetCurrenciesResponse> GetCurrencies(GetCurrenciesCriteria criteria);
    }
}
