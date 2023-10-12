using Dawem.Models.Dtos.Identity;

namespace Dawem.Models.Dtos.Provider
{
    public class UserBranchDTO
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserGlobalName { get; set; }

        public virtual SmartUserDTO? User { get; set; }

        public int BranchId { get; set; }
        public string? BranchGlobalName { get; set; }

        public virtual BranchDTO? branch { get; set; }
    }
}
