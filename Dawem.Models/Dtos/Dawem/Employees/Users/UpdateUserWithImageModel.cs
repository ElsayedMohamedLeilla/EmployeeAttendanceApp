using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class UpdateUserWithImageModel
    {
        public string UpdateUserModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
