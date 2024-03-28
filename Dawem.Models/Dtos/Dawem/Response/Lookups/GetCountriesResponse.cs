using Dawem.Models.Dtos.Dawem.Lookups;

namespace Dawem.Models.Response.Lookups
{
    public class GetCountriesResponse : BaseResponse
    {
        public List<CountryLiteDTO> Countries { get; set; }

    }
}
