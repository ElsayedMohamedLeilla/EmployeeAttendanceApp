namespace Dawem.Models.Dtos.Employees.User
{
    public class UserVerifyEmailModel
    {
        public int UserId { get; set; }
        public string VerificationCode { get; set; }
    }
}
