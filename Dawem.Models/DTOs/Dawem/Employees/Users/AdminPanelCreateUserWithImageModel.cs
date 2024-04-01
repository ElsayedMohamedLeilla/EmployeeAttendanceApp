using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class AdminPanelCreateUserWithImageModel
    {
        public string CreateUserModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
