using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class GetStoresResponse : BaseResponse
    {
        public List<StoreDTO> Stores { get; set; }
    }
}
