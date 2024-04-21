namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class UserSignUpModel
    {
         public string CompanyVerficationCode { get; set; }
        public int EmployeeNumber { get; set; }
        public int OTP { get; set; }
        //public string Name { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        //public int MobileCountryId { get; set; }
        //public string MobileNumber { get; set; }
    }
}
