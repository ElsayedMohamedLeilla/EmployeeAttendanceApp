using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Identity
{
    public class SignInModel
    {
        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int BranchId { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
