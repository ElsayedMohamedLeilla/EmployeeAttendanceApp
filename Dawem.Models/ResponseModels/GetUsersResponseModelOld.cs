using Dawem.Models.Dtos.Identities;

namespace Dawem.Models.ResponseModels
{
    public class GetUsersResponseModelOld
    {
        public List<UserDTO> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
