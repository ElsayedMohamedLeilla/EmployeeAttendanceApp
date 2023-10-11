using SmartBusinessERP.Models.Dtos.Identity;

namespace SmartBusinessERP.Models.Response.Identity
{
    public class GetSmartUserInfoResponse : BaseResponse
    {
        public SmartUserInfo? SmartUserInfo { get; set; }
    }
}
