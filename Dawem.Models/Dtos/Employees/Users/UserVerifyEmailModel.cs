namespace Dawem.Models.Dtos.Employees.Users
{
    public class UserVerifyEmailModel
    {
        public int UserId { get; set; }
        public string VerificationCode { get; set; }
    }
}
