using Dawem.Models.Dtos.Dawem.Lookups;

namespace Dawem.Models.Response.Dawem.Lookups
{
    public class GetCountriesResponse : BaseResponse
    {
        public List<CountryLiteDTO> Countries { get; set; }

    }
}
