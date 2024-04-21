namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class CreateEmployeeOTPDTO
    {
        public int EmployeeId { get; set; }
        public int OTP { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsVerified { get; set; }

    }
}
