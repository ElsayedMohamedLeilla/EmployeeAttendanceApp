using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Identities
{
    public class AdminPanelSignInModel
    {
        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
