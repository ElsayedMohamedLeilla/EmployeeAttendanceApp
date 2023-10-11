using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class GetStoreInfoResponse : BaseResponse
    {
        public StoreInfo? StoreInfo { get; set; }
    }
}
