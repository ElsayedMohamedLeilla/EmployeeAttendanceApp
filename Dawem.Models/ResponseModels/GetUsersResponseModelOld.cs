using Dawem.Models.Dtos.Identity;

namespace Dawem.Models.ResponseModels
{
    public class GetUsersResponseModelOld
    {
        public List<UserDTO> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
