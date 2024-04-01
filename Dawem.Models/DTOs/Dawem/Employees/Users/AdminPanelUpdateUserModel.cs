using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class AdminPanelUpdateUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImageName { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public List<int> Responsibilities { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
