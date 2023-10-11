using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class ProductUnitSearchResult : BaseResponse
    {
        public List<ProductUnitDto> ProductUnits { get; set; }

    }
}
