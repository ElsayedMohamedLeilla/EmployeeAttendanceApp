using Dawem.Models.Dtos.Identity;

namespace Dawem.Models.Response.Identity
{
    public class GetUserInfoResponse : BaseResponse
    {
        public SmartUserInfo? UserInfo { get; set; }
    }
}
