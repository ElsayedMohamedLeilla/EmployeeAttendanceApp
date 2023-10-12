using SmartBusinessERP.Models.Criteria.Lookups;
using SmartBusinessERP.Models.Response.Lookups;

namespace SmartBusinessERP.BusinessLogic.Lookups.Contract
{
    public interface ILookupsBL
    {
        Task<GetCountriesResponse> GetCountries(GetCountriesCriteria criteria);
        Task<GetCurrenciesResponse> GetCurrencies(GetCurrenciesCriteria criteria);
    }
}
