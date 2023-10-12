using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Response.Provider
{
    public class PackageSearchResult : BaseResponse
    {
        public List<PackageDto> Packages { get; set; }
    }
}
