using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class GetStoreInfoResponse : BaseResponse
    {
        public StoreInfo? StoreInfo { get; set; }
    }
}
