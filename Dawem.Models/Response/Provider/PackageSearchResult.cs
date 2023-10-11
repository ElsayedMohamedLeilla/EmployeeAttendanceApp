using SmartBusinessERP.Models.Dtos.Provider;

namespace SmartBusinessERP.Models.Response.Provider
{
    public class PackageSearchResult : BaseResponse
    {
        public List<PackageDto> Packages { get; set; }
    }
}
