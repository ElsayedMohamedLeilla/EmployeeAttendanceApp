using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.User
{
    public class CreateUserWithImageModel
    {
        public string CreateUserModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
