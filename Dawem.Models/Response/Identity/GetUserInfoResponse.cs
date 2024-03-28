using Dawem.Models.Dtos.Dawem.Identities;

namespace Dawem.Models.Response.Identity
{
    public class GetUserInfoResponse : BaseResponse
    {
        public UserInfo? UserInfo { get; set; }
    }
}
