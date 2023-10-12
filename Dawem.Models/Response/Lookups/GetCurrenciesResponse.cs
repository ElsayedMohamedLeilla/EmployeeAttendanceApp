using Dawem.Models.Dtos.Lookups;

namespace Dawem.Models.Response.Lookups
{
    public class GetCurrenciesResponse : BaseResponse
    {
        public List<CurrencyLiteDTO>? Currencies { get; set; }

    }
}
