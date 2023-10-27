using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees
{
    public class UpdateEmployeeWithImageModel
    {
        public string UpdateEmployeeModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
