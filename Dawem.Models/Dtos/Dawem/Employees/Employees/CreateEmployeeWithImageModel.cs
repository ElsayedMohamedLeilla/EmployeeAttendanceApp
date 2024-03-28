using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Employees
{
    public class CreateEmployeeWithImageModel
    {
        public string CreateEmployeeModelString { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
