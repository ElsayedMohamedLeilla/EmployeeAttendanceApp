namespace Dawem.Models.Dtos.Identity
{
    public class SignInModel
    {
        public bool RememberMe { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int BranchId { get; set; }
    }
}
