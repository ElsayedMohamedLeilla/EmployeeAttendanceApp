using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class AdminPanelCreateUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<int> Responsibilities { get; set; }
    }
}
