namespace Dawem.Models.Dtos.Identity
{
    public class TokenDto
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? UserFullName { get; set; }
        public string? BranchName { get; set; }
        public string? CompanyName { get; set; }
        public int CompanyId { get; set; }
        public int CurrentBranchId { get; set; }
        public bool IsMainBranch { get; set; }
        public string? Token { get; set; }
    }
}
