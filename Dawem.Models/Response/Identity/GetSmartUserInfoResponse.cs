using Dawem.Models.Dtos.Identity;

namespace Dawem.Models.Response.Identity
{
    public class GetSmartUserInfoResponse : BaseResponse
    {
        public SmartUserInfo? SmartUserInfo { get; set; }
    }
}
