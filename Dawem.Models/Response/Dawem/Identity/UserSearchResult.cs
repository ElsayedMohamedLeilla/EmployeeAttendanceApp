using Dawem.Models.Dtos.Dawem.Identities;

namespace Dawem.Models.Response.Dawem.Identity
{
    public class UserSearchResult : BaseResponse
    {
        public List<UserDTO> Users { get; set; }
    }
}
