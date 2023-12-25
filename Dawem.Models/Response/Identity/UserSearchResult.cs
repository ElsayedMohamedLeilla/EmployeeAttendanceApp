using Dawem.Models.Dtos.Identities;

namespace Dawem.Models.Response.Identity
{
    public class UserSearchResult : BaseResponse
    {
        public List<UserDTO>? Users { get; set; }
    }
}
