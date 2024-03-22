using Dawem.Models.Dtos.Providers;

namespace Dawem.Models.Response.Providers
{
    public class PackageSearchResult : BaseResponse
    {
        public List<PackageDto> Packages { get; set; }
    }
}
