using SmartBusinessERP.Models.Dtos.Lookups;

namespace SmartBusinessERP.Models.Response.Lookups
{
    public class GetCountriesResponse : BaseResponse
    {
        public List<CountryLiteDTO>? Countries { get; set; }

    }
}
