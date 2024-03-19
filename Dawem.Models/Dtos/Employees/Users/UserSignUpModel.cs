namespace Dawem.Models.Dtos.Employees.Users
{
    public class UserSignUpModel
    {
        public int CompanyId { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
    }
}
