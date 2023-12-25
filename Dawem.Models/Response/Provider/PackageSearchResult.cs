using Dawem.Models.Dtos.Providers;

namespace Dawem.Models.Response.Provider
{
    public class PackageSearchResult : BaseResponse
    {
        public List<PackageDto> Packages { get; set; }
    }
}
