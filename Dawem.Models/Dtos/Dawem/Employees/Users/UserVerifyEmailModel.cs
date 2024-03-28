namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class UserVerifyEmailModel
    {
        public int UserId { get; set; }
        public string VerificationCode { get; set; }
    }
}
