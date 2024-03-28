using Dawem.Models.Dtos.Dawem.Lookups;

namespace Dawem.Models.Response.Dawem.Lookups
{
    public class GetCurrenciesResponse : BaseResponse
    {
        public List<CurrencyLiteDTO> Currencies { get; set; }

    }
}
