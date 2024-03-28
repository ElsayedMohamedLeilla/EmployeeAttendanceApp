namespace Dawem.Models.Dtos.Dawem.Providers
{
    public class SignUpResponseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string Token { get; set; }
    }
}
