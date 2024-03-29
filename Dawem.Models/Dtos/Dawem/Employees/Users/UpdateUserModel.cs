using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
        public string ProfileImageName { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public List<int> Roles { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
