using Dawem.Contract.BusinessLogic.Lookups;
using Dawem.Models.Criteria.Lookups;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Lookups
{
    [Route(DawemKeys.ApiCcontrollerAction)]
    [ApiController]
    public class LookupsController : BaseController
    {
        private readonly ILookupsBL lookupsBL;


        public LookupsController(ILookupsBL _lookupsBL)
        {
            lookupsBL = _lookupsBL;
        }


        [HttpPost]
        public async Task<ActionResult> GetCountries(GetCountriesCriteria criteria)
        {
            var countriesRes = await lookupsBL.GetCountries(criteria);
            return Success(countriesRes);
        }

        [HttpPost]
        public async Task<ActionResult> GetCurrencies(GetCurrenciesCriteria criteria)
        {

            var currencieRes = await lookupsBL.GetCurrencies(criteria);
            return Success(currencieRes);
        }



    }
}