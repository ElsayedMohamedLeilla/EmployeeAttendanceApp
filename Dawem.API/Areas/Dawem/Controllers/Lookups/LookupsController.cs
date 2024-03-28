using Dawem.Contract.BusinessLogic.Lookups;
using Dawem.Models.Criteria.Lookups;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Lookups
{
    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    public class LookupsController : BaseController
    {
        private readonly ILookupsBL lookupsBL;


        public LookupsController(ILookupsBL _lookupsBL)
        {
            lookupsBL = _lookupsBL;
        }


        [HttpGet]
        public async Task<ActionResult> GetCountries([FromQuery] GetCountriesCriteria criteria)
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
        [HttpGet]
        public async Task<ActionResult> GetLanguages([FromQuery] GetLanguagesCriteria criteria)
        {
            var languagesReult = await lookupsBL.GetLanguages(criteria);
            return Success(languagesReult);
        }


    }
}