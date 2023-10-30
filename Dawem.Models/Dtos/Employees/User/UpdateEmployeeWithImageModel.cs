using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.User
{
    public class UpdateUserWithImageModel
    {
        public string UpdateUserModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
