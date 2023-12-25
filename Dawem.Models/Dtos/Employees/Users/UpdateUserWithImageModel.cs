using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Users
{
    public class UpdateUserWithImageModel
    {
        public string UpdateUserModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
