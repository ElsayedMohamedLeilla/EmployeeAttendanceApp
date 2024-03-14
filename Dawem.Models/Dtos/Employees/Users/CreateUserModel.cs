using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Users
{
    public class CreateUserModel
    {
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<int> Roles { get; set; }
    }
}
