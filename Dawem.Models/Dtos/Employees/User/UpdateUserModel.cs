using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.User
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProfileImageName { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
