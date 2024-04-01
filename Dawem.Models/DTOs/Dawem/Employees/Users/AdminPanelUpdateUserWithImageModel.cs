using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class AdminPanelUpdateUserWithImageModel
    {
        public string UpdateUserModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
