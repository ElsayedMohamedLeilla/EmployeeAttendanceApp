using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Users
{
    public class CreateUserWithImageModel
    {
        public string CreateUserModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
