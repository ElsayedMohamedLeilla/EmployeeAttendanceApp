using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class ProductUnitSearchResult : BaseResponse
    {
        public List<ProductUnitDto> ProductUnits { get; set; }

    }
}
