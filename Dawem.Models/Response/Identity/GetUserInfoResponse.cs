using Dawem.Models.Dtos.Identity;

namespace Dawem.Models.Response.Identity
{
    public class GetUserInfoResponse : BaseResponse
    {
        public UserInfo? UserInfo { get; set; }
    }
}
