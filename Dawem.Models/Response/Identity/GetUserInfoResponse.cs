using Dawem.Models.Dtos.Identities;

namespace Dawem.Models.Response.Identity
{
    public class GetUserInfoResponse : BaseResponse
    {
        public UserInfo? UserInfo { get; set; }
    }
}
