using Dawem.Models.Dtos.Dawem.Providers;

namespace Dawem.Models.Response.Providers
{
    public class PackageSearchResult : BaseResponse
    {
        public List<PackageDto> Packages { get; set; }
    }
}
