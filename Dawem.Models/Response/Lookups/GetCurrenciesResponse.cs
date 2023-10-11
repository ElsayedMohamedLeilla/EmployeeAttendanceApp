using SmartBusinessERP.Models.Dtos.Lookups;

namespace SmartBusinessERP.Models.Response.Lookups
{
    public class GetCurrenciesResponse : BaseResponse
    {
        public List<CurrencyLiteDTO>? Currencies { get; set; }

    }
}
