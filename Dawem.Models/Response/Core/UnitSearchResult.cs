using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class UnitSearchResult : BaseResponse
    {
        public List<UnitDTO?>? Units { get; set; }
    }
}
