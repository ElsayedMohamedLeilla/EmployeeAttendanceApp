using Dawem.Models.Dtos.Dawem.Identities;

namespace Dawem.Models.Dtos.Dawem.Providers
{
    public class UserBranchDTO
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserGlobalName { get; set; }

        public virtual UserDTO User { get; set; }

        public int BranchId { get; set; }
        public string BranchGlobalName { get; set; }
    }
}
