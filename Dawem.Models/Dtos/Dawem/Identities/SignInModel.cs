using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Identities
{
    public class SignInModel
    {
        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CompanyId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string FCMToken { get; set; }
        public string FingerprintMobileCode { get; set; }
    }
}
